using Project21032025.Models;
using Project21032025.Models.DTO;

namespace Project21032025.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserDTO> RegisterUser(User user);
        Task<LoginResponseDTO> LoginUser(LoginRequestDTO loginRequest);
    }
}
