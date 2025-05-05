using LeaveManagement.Infrastructure.DataModel;
using LeaveManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context; 
        }

        public async Task<(User?, string)> LoginUser(LoginRequestData loginRequest)
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == loginRequest.Email && x.Password == loginRequest.Password);

            if (user == null)
            {
                return (null, string.Empty);
            }

            string role = "User"; 
            if (user.Email.ToLower() == "admin@gmail.com")
            {
                role = "Admin";
            }
            else if (await _context.Managers.AnyAsync(m => m.UserId == user.Id))
            {
                role = "Manager";
            }
            else if (await _context.Employees.AnyAsync(e => e.UserId == user.Id))
            {
                role = "Employee";
            }

            return (user, role);
        }
    }
}
