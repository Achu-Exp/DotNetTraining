using LeaveManagement.Infrastructure.DataModel;

namespace LeaveManagement.Infrastructure.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeData>> GetAllAsync();
        Task<EmployeeData?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task<Employee> UpdateAsync(Employee employee);
        Task<bool> DeleteAsync(Employee? employee);
        Task<Employee?> FindAsync(int id);

        //IQueryable<Employee> GetQueryable(Expression<Func<Employee, bool>> expression);
    }
}
