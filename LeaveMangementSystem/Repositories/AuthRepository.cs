using AutoMapper;
using LeaveMangementSystem.Data;
using LeaveMangementSystem.Models;
using LeaveMangementSystem.Models.DTO;
using LeaveMangementSystem.Repositories.Interfaces;

namespace LeaveMangementSystem.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;

        public AuthRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
            _secretKey = Environment.GetEnvironmentVariable("API_Secret");
        }

        public async Task<User> LoginUser(LoginRequestDTO loginRequestDTO)
        {
            User user =  _db.Users.FirstOrDefault(x => x.Email == loginRequestDTO.Email);
            if (user == null)
            {
                return null;
            }
            else
            {
                return user;
            }
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
