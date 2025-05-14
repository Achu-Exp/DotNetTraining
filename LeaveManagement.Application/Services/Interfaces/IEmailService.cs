namespace LeaveManagement.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string receptor, string subject, string body);

    }
}
