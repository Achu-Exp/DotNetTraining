namespace LeaveManagement.Application.Services.Interfaces
{
    public interface IUserService
    {

        Task<DTO.UserDTO> AddUserByAsync(DTO.UserDTO user);
        Task<DTO.UserDTO> GetUserByIdAsync(int id);
        Task<List<DTO.UserDTO>> GetUsersAsync();
        Task<DTO.UserDTO> UpdateUserAsync(DTO.UserDTO user);
        Task<int> DeleteUserAsync(int id);
    }
}
