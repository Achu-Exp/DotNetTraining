namespace LeaveManagement.Application.Services.Interfaces
{
    public interface IManagerSevice
    {
        Task<DTO.ManagerDTO> AddManagerByAsync(DTO.ManagerDTO manager);
        Task<DTO.ManagerDTO> GetManagerByIdAsync(int id);
        Task<List<DTO.ManagerDTO>> GetManagersAsync();
        Task<DTO.ManagerDTO> UpdateManagerAsync(DTO.ManagerDTO manager);
        Task<int> DeleteManagerAsync(int id);
    }
}
