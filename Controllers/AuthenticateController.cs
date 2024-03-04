using ApiAppBanSach.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using SendGrid.Helpers.Mail;
using SendGrid;
using ApiAppBangHang.Interface;
using System.Net;
using ApiAppBangHang.Models;
using ApiAppBangHang;


namespace ApiAppBanSach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private UnitOfWork _unitOfWork;

        public AuthenticateController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IEmailSender emailSender,
            SignInManager<IdentityUser> signInManager,
            UnitOfWork unit
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _unitOfWork = unit;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
                var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                var isLockedOut = await _userManager.IsLockedOutAsync(user);
                string failure;

                HttpContext.Request.Cookies.TryGetValue("LoginInfo", out failure);
                int fail = Convert.ToInt32(failure);

                if (user != null && isPasswordCorrect && isEmailConfirmed && !isLockedOut && fail <= 5)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = GetToken(authClaims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                else if (fail >= 5)
                {
                    fail = 0;
                    await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddMinutes(30));
                    HttpContext.Response.Cookies.Append("LoginInfo", fail.ToString());
                    return base.StatusCode(StatusCodes.Status409Conflict, new Models.Response { Status = "Error", Message = "Đã vượt quá số lần đăng nhập! Thử lại sau 30 phút" });
                }
                else if (isLockedOut)
                {
                    return base.StatusCode(StatusCodes.Status409Conflict, new Models.Response { Status = "Error", Message = "Vui lòng thử lại sau ít phút" });
                }
                else if (user == null)
                {
                    fail += 1;
                    HttpContext.Response.Cookies.Append("LoginInfo", fail.ToString());
                    return base.StatusCode(StatusCodes.Status500InternalServerError, new Models.Response { Status = "Error", Message = "Tài khoản không tồn tại!" });
                }
                else if (!isPasswordCorrect)
                {
                    fail += 1;
                    HttpContext.Response.Cookies.Append("LoginInfo", fail.ToString());
                    return base.StatusCode(StatusCodes.Status500InternalServerError, new Models.Response { Status = "Error", Message = "Sai tài khoản hoặc mật khẩu" });
                }
                else if (!isEmailConfirmed)
                {
                    fail += 1;
                    HttpContext.Response.Cookies.Append("LoginInfo", fail.ToString());
                    return base.StatusCode(StatusCodes.Status500InternalServerError, new Models.Response { Status = "Error", Message = "Tài khoản chưa xác thực email! Kiểm tra email của bạn để tiến hành xác thực" });
                }
                return Unauthorized();
            }
            catch (Exception ex) 
            {
                return base.StatusCode(StatusCodes.Status500InternalServerError, new Models.Response { Status = "Error", Message = ex.Message});
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try {
                if (!string.IsNullOrWhiteSpace(model.Username) && !string.IsNullOrWhiteSpace(model.Password) && !string.IsNullOrWhiteSpace(model.Email))
                {
                    var userExists = await _userManager.FindByNameAsync(model.Username);
                    if (userExists != null)
                        return base.StatusCode(StatusCodes.Status500InternalServerError, new Models.Response { Status = "Error", Message = "Tài khoản đã tồn tại!" });

                    var emailExists = await _userManager.FindByEmailAsync(model.Email);
                    if (emailExists != null)
                        return base.StatusCode(StatusCodes.Status500InternalServerError, new Models.Response { Status = "Error", Message = "Email đã tồn tại!" });

                    IdentityUser user = new()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.Username,
                        Id = Guid.NewGuid().ToString()
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                        return base.StatusCode(StatusCodes.Status500InternalServerError, new Models.Response { Status = "Error", Message = "Đăng ký thất bại! Kiểm tra lại thông tin đăng ký" });

                    //thông tin người dùng app
                    AppUser appUser = new AppUser();
                    appUser.AppUserId = user.Id;
                    appUser.EmailAddress = user.Email;
                    appUser.Updated = DateTime.Now;
                    
                    //tạo người dùng app
                    _unitOfWork.AppUserRepository.Insert(appUser);
                    _unitOfWork.Save();


                    //tạo token xác thực email
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Authenticate", new {token, userId = user.Id}, Request.Scheme);
                    SendGrid.Response response = await _emailSender.SendEmailConfirmationAsync(user, confirmationLink);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        return base.Ok(new Models.Response { Status = "Success", Message = "Đăng ký thành công! Kiểm tra email xác thực" });
                    }
                    
                    return base.StatusCode(StatusCodes.Status500InternalServerError, new Models.Response { Status = "Error", Message = "Có lỗi xảy ra khi gửi email"});
                }
                else
                {
                    return base.StatusCode(StatusCodes.Status500InternalServerError, new Models.Response { Status = "Error", Message = "Thiếu thông tin tài khoản, mật khẩu, email" });
                }
            }catch(Exception ex)
            {
                return base.StatusCode(StatusCodes.Status500InternalServerError, new Models.Response { Status = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return base.StatusCode(StatusCodes.Status500InternalServerError, new Models.Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return base.StatusCode(StatusCodes.Status500InternalServerError, new Models.Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return base.Ok(new Models.Response { Status = "Success", Message = "User created successfully!" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddDays(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

       
        public async Task<ContentResult> ConfirmEmail(string token, string userId)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);
            bool isEmailConfirmed  = await _userManager.IsEmailConfirmedAsync(identityUser);
            if (isEmailConfirmed) 
            {
                return base.Content("<p>Email đã được xác thực trước đó</p>", "text/html", Encoding.UTF8);
            }

            if (userId == null || token == null)
            {
                return base.Content("<p>Có lỗi xảy ra</p>", "text/html", Encoding.UTF8);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return base.Content("<p>Có lỗi xảy ra</p>", "text/html", Encoding.UTF8);
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {

                return base.Content("<p>Xác thực thành công! Hãy đăng nhập vào ứng dụng</p>", "text/html", Encoding.UTF8);
            }
            return base.Content("<p>Có lỗi xảy ra</p>", "text/html", Encoding.UTF8);
        }

        [HttpPost]
        [Route("forgotpassword")]
        public async Task<ContentResult> ForgotPassword([FromBody] ResetPasswordModel model)  
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return base.Content("<p>Email không tồn tại trên hệ thống</p>", "text/html", Encoding.UTF8);
            }
            else if (user != null)
            {
                bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                if (!isEmailConfirmed)
                {
                    return base.Content("<p>Email chưa được xác thực</p>", "text/html", Encoding.UTF8);
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callback = Url.Action("ResetPassword", "Authenticate", new { token, userId = user.Id, confirmedPassword = model.ConfirmPassword }, Request.Scheme);
                SendGrid.Response res = await _emailSender.SendEmailResetPasswordAsync(user, callback);
                if (res.IsSuccessStatusCode) 
                {
                    return base.Content("<p>Kiểm tra email của bạn để đặt lại mật khẩu</p>", "text/html", Encoding.UTF8);
                }
            }
            return base.Content("<p>Có lỗi xảy ra</p>", "text/html", Encoding.UTF8);
        }

        [Route("resetpassword")]
        public async Task<ContentResult> ResetPassword(string token, string userId, string confirmedPassword)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ResetPasswordAsync(identityUser, token, confirmedPassword);
            if (!result.Succeeded) 
            {
                return base.Content("<p>Có lỗi xảy ra</p>", "text/html", Encoding.UTF8);
            }
            return base.Content("<p>Đặt lại mật khẩu thành công</p>", "text/html", Encoding.UTF8);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return new OkObjectResult(new { message = "Đăng xuất thành công" });
        }
    }
}
