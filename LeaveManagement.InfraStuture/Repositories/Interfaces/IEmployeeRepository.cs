using LeaveManagement.Infrastructure.DataModel;

namespace LeaveManagement.Infrastructure.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeData>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        Task<EmployeeData?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task<Employee> UpdateAsync(Employee employee);
        Task<bool> DeleteAsync(Employee? employee);
        Task<Employee?> FindAsync(int id);
        Task<List<EmployeeData>> GetEmployeeByDepartmentId(int id);
        Task<List<EmployeeData>> GetAllEmployeesByManagerId(int id);


        //IQueryable<Employee> GetQueryable(Expression<Func<Employee, bool>> expression);
    }
}
