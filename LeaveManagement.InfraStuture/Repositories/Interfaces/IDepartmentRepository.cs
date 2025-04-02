using LeaveManagement.Infrastructure.DataModel;

namespace LeaveManagement.Infrastructure.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<List<DepartmentData>> GetAllAsync();
        Task<DepartmentData?> GetByIdAsync(int id);
        Task AddAsync(Department department);
        Task<Department> UpdateAsync(Department department);
        Task<bool> DeleteAsync(Department? department);
        Task<Department?> FindAsync(int id);
    }
}
