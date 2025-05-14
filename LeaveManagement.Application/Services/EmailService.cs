using System.Net;
using System.Net.Mail;
using LeaveManagement.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LeaveManagement.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmail(string receptor, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(receptor))
            {
                throw new ArgumentException("Receptor email cannot be null or empty", nameof(receptor));
            }

            try
            {
                // Validate Email Format
                var mailAddress = new MailAddress(receptor);
            }
            catch (FormatException)
            {
                throw new ArgumentException($"Invalid email format: {receptor}");
            }

            var host = configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");
            string email = Environment.GetEnvironmentVariable("EMAIL");
            string password = Environment.GetEnvironmentVariable("PASSWORD");

            using var smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email, password)
            };

            using var message = new MailMessage(email!, receptor, subject, body)
            {
                IsBodyHtml = true
            };
            await smtpClient.SendMailAsync(message);
        }
    }
}
