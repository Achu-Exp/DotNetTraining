using LeaveManagement.Infrastructure.DataModel;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
        }


        public async Task<Department?> FindAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<List<DepartmentData>> GetAllAsync()
        {
            return await _context.Departments.Select(e => new DepartmentData
                (
                    e.Id,
                    e.Name,
                    e.Description
                
                )
                ).ToListAsync();
        }

        public async Task<DepartmentData?> GetByIdAsync(int id)
        {
            return await _context.Departments
               .Where(e => e.Id == id)
               .Select(e => new DepartmentData
                  (
                    e.Id,
                    e.Name,
                    e.Description

                   )
               ).FirstOrDefaultAsync();
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            return await Task.FromResult(department);
        }
        public async Task<bool> DeleteAsync(Department? department)
        {
            _context.Departments.Remove(department);
            return await Task.FromResult(true);
        }
    }
}
