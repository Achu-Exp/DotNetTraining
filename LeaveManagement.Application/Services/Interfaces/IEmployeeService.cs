namespace LeaveManagement.Application.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<DTO.EmployeeDTO> AddEmployeeByAsync(DTO.EmployeeDTO employee);
        Task<DTO.EmployeeDTO> GetEmployeeByIdAsync(int id);
        Task<List<DTO.EmployeeDTO>> GetEmployeesAsync();
        Task<DTO.EmployeeDTO> UpdateEmployeeAsync(DTO.EmployeeDTO employee);
        Task<int> DeleteEmployeeAsync(int id);
        Task<List<DTO.EmployeeDTO>> GetEmployeeByDepartmentId(int id);
        Task<List<DTO.EmployeeDTO>> GetAllEmployeesByManagerId(int id);
    }
}
