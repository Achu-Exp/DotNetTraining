using LeaveMangementSystem.Models;
using LeaveMangementSystem.Models.DTO;

namespace LeaveMangementSystem.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserDTO> RegisterUser(User user);
        Task<User> LoginUser(LoginRequestDTO loginRequestDTO);
    }
}
