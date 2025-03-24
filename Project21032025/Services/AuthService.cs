using Project21032025.Models;
using Project21032025.Models.DTO;
using Project21032025.Repositories.Interfaces;

namespace Project21032025.Services
{
    public class AuthService
    {
        public readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<UserDTO> RegisterUser(RegRequestDTO regRequestDTO)
        {
            User user = new()
            {
                FullName = regRequestDTO.FullName,
                Email = regRequestDTO.Email,
                Password = regRequestDTO.Password,
                Role = regRequestDTO.Role
            };
           return await _authRepository.RegisterUser(user);
        }
    }
}
