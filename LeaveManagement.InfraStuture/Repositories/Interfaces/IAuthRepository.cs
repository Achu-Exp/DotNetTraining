using LeaveManagement.Infrastructure.DataModel;

namespace LeaveManagement.Infrastructure.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<(User?, string)> LoginUser(LoginRequestData loginRequest);
    }
}
