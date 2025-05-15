namespace LeaveManagement.Application.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<DTO.EmployeeDTO> AddEmployeeByAsync(DTO.EmployeeDTO employee);
        Task<DTO.EmployeeDTO> GetEmployeeByIdAsync(int id);
        Task<List<DTO.EmployeeDTO>> GetEmployeesAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        Task<DTO.EmployeeDTO> UpdateEmployeeAsync(DTO.EmployeeDTO employee);
        Task<int> DeleteEmployeeAsync(int id);
        Task<List<DTO.EmployeeDTO>> GetEmployeeByDepartmentId(int id);
        Task<List<DTO.EmployeeDTO>> GetAllEmployeesByManagerId(int id);
    }
}
