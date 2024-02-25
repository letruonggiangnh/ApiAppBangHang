using ApiAppBangHang.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ApiAppBangHang.Implements
{
    public class EmailSender : IEmailSender
    {
        public IConfiguration Configuration { get; }
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly EmailAddress _fromEmail;
        
        public EmailSender(IConfiguration configuration, IUrlHelperFactory urlHelperFactory)
        {
            Configuration = configuration;
            _urlHelperFactory = urlHelperFactory;
            _fromEmail = new EmailAddress(Configuration.GetSection("EmailConfiguration:From").Value);
        }
        public async Task<Response> SendEmailConfirmationAsync(IdentityUser user, string confirmationLink)
        {
            var emailMessage = CreateEmailMessage(user.Email, confirmationLink);
            Response res = await Send(emailMessage);
            return res;
        }

        private SendGridMessage CreateEmailMessage(string toEmail, string confirmationLink)
        {
            string from = Configuration.GetSection("EmailConfiguration:From").Value;
            var fromEmail = new EmailAddress(from);
            var to = new EmailAddress(toEmail);
            var subject = "Xác nhận Email";
            var plainTextContent = "Vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết sau đây: " + confirmationLink;
            var htmlContent = "<p>Vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết sau đây:</p><p><a href=\"" + confirmationLink + "\">Xác nhận Email</a></p>";
            var msg = MailHelper.CreateSingleEmail(fromEmail, to, subject, plainTextContent, htmlContent);
            return msg;
        }
        private async Task<Response> Send(SendGridMessage mailMessage)
        {
            var sendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(sendGridApiKey);
            Response res = await client.SendEmailAsync(mailMessage);
            return res;
        }

        public async Task<Response> SendEmailResetPasswordAsync(IdentityUser user, string resetLink)
        {
            var emailMessage = CreateResetPasswordEmailMessage(user.Email, resetLink);
            Response res = await Send(emailMessage);
            return res;
        }

        private SendGridMessage CreateResetPasswordEmailMessage(string email, string resetLink)
        {
            var to = new EmailAddress(email);
            var subject = "Đặt lại mật khẩu";
            var plainTextContent = "Vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết sau đây: " + resetLink;
            var htmlContent = "<p>Vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết sau đây:</p><p><a href=\"" + resetLink + "\">Đặt lại mật khẩu</a></p>";
            var msg = MailHelper.CreateSingleEmail(_fromEmail, to, subject, plainTextContent, htmlContent);
            return msg;
        }
    }
}
