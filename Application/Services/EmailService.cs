using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using FengShuiWeb.Infrastructure;
using FengShuiWeb.Application.Interfaces;

namespace FengShuiWeb.Application
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value ?? throw new ArgumentNullException(nameof(smtpSettings));
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new System.Net.NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.UseSSL
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username, _smtpSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);
            await smtpClient.SendMailAsync(mailMessage);
        }
        public async Task SendEmailConfirmationAsync(string to, string token)
        {
            var confirmationLink = $"{Environment.GetEnvironmentVariable("CLIENT_URL")}/confirm-email?token={token}";
            var htmlTemplate = await System.IO.File.ReadAllTextAsync("Templates/EmailConfirmationTemplate.html");
            var emailBody = htmlTemplate.Replace("{{confirmationLink}}", confirmationLink);

            var message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username, _smtpSettings.FromName),
                Subject = "Confirm Your Email",
                Body = emailBody,
                IsBodyHtml = true
            };
            message.To.Add(to);

            using (var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                smtpClient.Credentials = new System.Net.NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                smtpClient.EnableSsl = _smtpSettings.UseSSL;
                if (_smtpSettings.UseTLS)
                    smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(message);
            }
        }

        public async Task SendResetPasswordEmailAsync(string to, string token)
        {
            var resetLink = $"{Environment.GetEnvironmentVariable("CLIENT_URL")}/reset-password?token={token}";
            var htmlTemplate = await System.IO.File.ReadAllTextAsync("Templates/ResetPasswordTemplate.html");
            var emailBody = htmlTemplate.Replace("{{resetPasswordLink}}", resetLink);

            var message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username, _smtpSettings.FromName),
                Subject = "Reset Your Password",
                Body = emailBody,
                IsBodyHtml = true
            };
            message.To.Add(to);

            using (var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                smtpClient.Credentials = new System.Net.NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                smtpClient.EnableSsl = _smtpSettings.UseSSL;
                if (_smtpSettings.UseTLS)
                    smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(message);
            }
        }
    }
}