using System.Net.Mail;
using System.Net;
using LeaveMangementSystem.Services.Interfaces;

namespace LeaveMangementSystem.Services
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
