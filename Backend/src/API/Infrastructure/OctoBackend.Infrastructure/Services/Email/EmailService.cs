using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using OctoBackend.Application.Abstractions.Services.Email;
using OctoBackend.Application.Models;

namespace OctoBackend.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(EmailBox box)
        {
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.zoho.eu", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["Email:Username"], _config["Email:Password"]);

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_config["Email:Username"]));
            message.Subject = box.Subject;

            foreach (var email in box.Emails)
            {
                message.To.Add(MailboxAddress.Parse(email.EmailAdress));
                message.Body = new TextPart(TextFormat.Html) { Text = email.Body };
                await smtp.SendAsync(message);
            }

            smtp.Disconnect(true);
        }
    }
}
