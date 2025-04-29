using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using LeaveManagement.Application.DTO;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LeaveManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        public readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private string secretKey;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _authRepository = authRepository;
            _mapper = mapper;
            secretKey = Environment.GetEnvironmentVariable("API_SECRET");

        }

        public async Task<LoginResponseDTO> LoginUser(LoginRequestDTO loginRequest)
        {
            var loginRequestData = _mapper.Map<LoginRequestData>(loginRequest);
            var (user,role) = await _authRepository.LoginUser(loginRequestData);

            if (user == null)
            {
                throw new Exception("Invalid email or password.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);


            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, role)

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
