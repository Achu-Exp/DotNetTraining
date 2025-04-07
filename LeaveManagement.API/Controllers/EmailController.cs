using LeaveManagement.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers
{
    [Route("api/emails")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;

        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;

            _secretKey = Environment.GetEnvironmentVariable("API_SECRET");
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(string receptor, string subject, string body)
        {
            await emailService.SendEmail(receptor, subject, body);
            return Ok();
        }
    }
}
