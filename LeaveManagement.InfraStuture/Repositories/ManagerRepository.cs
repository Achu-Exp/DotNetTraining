using LeaveManagement.Infrastructure.DataModel;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly ApplicationDbContext _context;

        public ManagerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Manager manager)
        {
            if (manager.User != null)
            {
                manager.User.Password = "experion@123";
                await _context.Users.AddAsync(manager.User);
                manager.UserId = manager.User.Id;
            }
            await _context.Managers.AddAsync(manager);
        }

        public async Task<Manager?> FindAsync(int id)
        {
            return await _context.Managers
             .Include(m => m.User)
             .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<ManagerData>> GetAllAsync()
        {
            return await _context.Managers
              .Include(m => m.User)
              .Select(m => new ManagerData
              (
                  m.Id,
                 new UserData( 
                        m.User.Id,
                        m.User.Name,
                        m.User.Email,
                        m.User.Address,
                        m.User.DepartmentId
                    )
              ))
              .ToListAsync();
        }

        public async Task<ManagerData?> GetByIdAsync(int id)
        {
            return await _context.Managers
            .Include(m => m.User)
            .Where(m => m.Id == id)
            .Select(m => new ManagerData
            (
                  m.Id,
                  new UserData(  
                        m.User.Id,
                        m.User.Name,
                        m.User.Email,
                        m.User.Address,
                        m.User.DepartmentId
                    )
            ))
            .FirstOrDefaultAsync();
        }

        public async Task<Manager> UpdateAsync(Manager manager)
        {

            var existingUser = await _context.Users.FindAsync(manager.UserId);
            if (existingUser != null)
            {
                existingUser.Name = manager.User.Name;
                existingUser.Email = manager.User.Email;
                _context.Users.Update(existingUser);
            }
            _context.Managers.Update(manager);
            return await Task.FromResult( manager );
        }
        public async Task<bool> DeleteAsync(Manager? manager)
        {

            if (manager == null)
                return false;

            var user = await _context.Users.FindAsync(manager.UserId);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
             _context.Managers.Remove(manager);
            return await Task.FromResult ( true );
        }
    }
}
