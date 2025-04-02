using LeaveManagement.Infrastructure.DataModel;

namespace LeaveManagement.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserData>> GetAllAsync();
        Task<UserData?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(User? user);
        Task<User?> FindAsync(int id);
    }
}
