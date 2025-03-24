using AutoMapper;
using Project21032025.Data;
using Project21032025.Models;
using Project21032025.Models.DTO;
using Project21032025.Repositories.Interfaces;

namespace Project21032025.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private string secretKey;

        public AuthRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
            secretKey = _configuration.GetValue<string>("ApiSettings:Secret");
        }

        public Task<LoginResponseDTO> LoginUser(LoginRequestDTO loginRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> RegisterUser(User user)
        {
            await _db.Users.AddAsync(user);
            _db.SaveChanges();
            user.Password = "";
            return _mapper.Map<UserDTO>(user);
        }
    }
}
