namespace LeaveManagement.Application.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<DTO.DepartmentDTO> AddDepartmentByAsync(DTO.DepartmentDTO department);
        Task<DTO.DepartmentDTO> GetDepartmentByIdAsync(int id);
        Task<List<DTO.DepartmentDTO>> GetDepartmentsAsync();
        Task<DTO.DepartmentDTO> UpdateDepartmentAsync(DTO.DepartmentDTO department);
        Task<int> DeleteDepartmentAsync(int id);
    }
}
