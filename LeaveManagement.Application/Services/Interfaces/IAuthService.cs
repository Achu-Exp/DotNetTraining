namespace LeaveManagement.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<DTO.LoginResponseDTO> LoginUser(DTO.LoginRequestDTO loginRequest);
    }
}
