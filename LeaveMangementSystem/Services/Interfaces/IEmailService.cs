namespace LeaveMangementSystem.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string receptor, string subject, string body);

    }
}
