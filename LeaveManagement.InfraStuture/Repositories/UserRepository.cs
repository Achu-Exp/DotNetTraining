using LeaveManagement.Infrastructure.DataModel;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> FindAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<UserData>> GetAllAsync()
        {
            return await _context.Users
                .Select(u => new UserData
                (
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Address,
                    u.DepartmentId
                ))
                .ToListAsync();
        }

        public async Task<UserData?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserData
                (
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Address,
                    u.DepartmentId
                ))
                .FirstOrDefaultAsync();
        }
        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return await   Task.FromResult(user) ;
        }

        public async Task<bool> DeleteAsync(User? user)
        {
            var entity = _context.Users.Remove(user);
            return await Task.FromResult(true);
        }
    }
}
