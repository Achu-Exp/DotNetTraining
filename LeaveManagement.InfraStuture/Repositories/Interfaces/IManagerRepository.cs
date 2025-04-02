using LeaveManagement.Infrastructure.DataModel;

namespace LeaveManagement.Infrastructure.Repositories.Interfaces
{
    public interface IManagerRepository
    {
        Task<List<ManagerData>> GetAllAsync();
        Task<ManagerData?> GetByIdAsync(int id);
        Task AddAsync(Manager manager);
        Task<Manager> UpdateAsync(Manager manager);
        Task<bool> DeleteAsync(Manager? manager);
        Task<Manager?> FindAsync(int id);
    }
}
