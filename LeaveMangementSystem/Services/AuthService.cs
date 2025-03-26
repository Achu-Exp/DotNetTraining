using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LeaveMangementSystem.Models;
using LeaveMangementSystem.Models.DTO;
using LeaveMangementSystem.Repositories.Interfaces;

namespace LeaveMangementSystem.Services
{
    public class AuthService
    {
        public readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private string secretKey;
        public AuthService(IAuthRepository authRepository, IConfiguration configuration, IMapper mapper)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _configuration = configuration;
            secretKey = Environment.GetEnvironmentVariable("API_Secret");
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

        public async Task<LoginResponseDTO> LoginUser(LoginRequestDTO loginRequestDTO)
        {
            User user = await _authRepository.LoginUser(loginRequestDTO);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);


            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
            };
          

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new()
            {
                User = _mapper.Map<UserDTO>(user),
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            return loginResponseDTO;
        }
    }
}
