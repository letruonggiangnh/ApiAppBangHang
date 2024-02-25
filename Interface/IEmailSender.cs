using SendGrid;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading.Tasks;

namespace ApiAppBangHang.Interface
{
    public interface IEmailSender
    {
        Task<Response> SendEmailConfirmationAsync(IdentityUser user,  string confirmationLink);
        Task<Response> SendEmailResetPasswordAsync(IdentityUser user, string resetLink);
    }
}
