using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            string email = Environment.GetEnvironmentVariable("EMAIL");
            string password = Environment.GetEnvironmentVariable("PASSWORD");
            smtpClient.Credentials = new NetworkCredential(email, password);

            var message = new MailMessage(email!, receptor, subject, body);
            message.IsBodyHtml = true;
            await smtpClient.SendMailAsync(message);

        }
    }
}
