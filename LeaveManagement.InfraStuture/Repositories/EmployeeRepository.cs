﻿using LeaveManagement.Infrastructure.DataModel;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Employee employee)
        {
            if (employee.User != null)
            {
                employee.User.Password = "experion@123";
                await _context.Users.AddAsync(employee.User);
                employee.UserId = employee.User.Id; 
            }
            await _context.Employees.AddAsync(employee);
        }

        public async Task<Employee?> FindAsync(int id)
        {
            return await _context.Employees
                 .Include(e => e.User)
                 .FirstOrDefaultAsync(e => e.Id == id);
        }


        public async Task<List<EmployeeData>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var employees = _context.Employees
                .Include(e => e.User)
                .Select(x => new EmployeeData
                (
                 x.Id,
                 new UserData(
                     x.User.Id,
                     x.User.Name,
                     x.User.Email,
                     x.User.Address,
                     x.User.DepartmentId
                     ),
                 x.ManagerId
                ))
                .AsQueryable();

            // Filtering
            if(!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    employees = employees.Where(x=>x.User.Name == filterQuery);
                }
            }

            //Sorting
            if(!string.IsNullOrEmpty(sortBy))
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    employees = isAscending ? employees.OrderBy(x=>x.User.Name): employees.OrderByDescending(x=>x.User.Name);
                }
                else if(sortBy.Equals("Address", StringComparison.OrdinalIgnoreCase))
                {
                    employees = isAscending ? employees.OrderBy(x=>x.User.Address) : employees.OrderByDescending(x => x.User.Address);
                }
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await employees.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<EmployeeData?> GetByIdAsync(int id)
        {
            return await _context.Employees
            .Include(e => e.User)
            .Where(e => e.Id == id)
            .Select(x => new EmployeeData
                 (
                    x.Id,
                    new UserData(  // Assuming you have a UserData DTO
                        x.User.Id,
                        x.User.Name,
                        x.User.Email,
                        x.User.Address,
                         x.User.DepartmentId
                    ),
                    x.ManagerId
                 )
            ).FirstOrDefaultAsync();
        }
       

        public async Task<Employee> UpdateAsync(Employee employee)
        {

            var existingUser = await _context.Users.FindAsync(employee.UserId);
            if (existingUser != null)
            {
                existingUser.Name = employee.User.Name;
                existingUser.Email = employee.User.Email;
                existingUser.Address = employee.User.Address;
                _context.Users.Update(existingUser);
            }
            _context.Employees.Update(employee);
            return await Task.FromResult(employee);
        }
        public async Task<bool> DeleteAsync(Employee? employee)
        {

            if (employee == null)
                return false;

            var user = await _context.Users.FindAsync(employee.UserId);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            _context.Employees.Remove(employee);
            return await Task.FromResult(true);
        }

        public async Task<List<EmployeeData>> GetEmployeeByDepartmentId(int id)
        {
            return await _context.Employees
                .Where(e=>e.User.DepartmentId == id)
                .Select(e=> new EmployeeData(
                    e.Id,
                    new UserData(
                        e.User.Id,
                        e.User.Name,
                        e.User.Email,
                        e.User.Address,
                        e.User.DepartmentId
                    ),
                    e.ManagerId
                    )).ToListAsync();
        }

        public async Task<List<EmployeeData>> GetAllEmployeesByManagerId(int id)
        {
            return await _context.Employees
                .Where(e => e.ManagerId == id)
                .Select(e => new EmployeeData(
                    e.Id,
                    new UserData(
                        e.User.Id,
                        e.User.Name,
                        e.User.Email,
                        e.User.Address,
                        e.User.DepartmentId
                    ),
                    e.ManagerId
                    )).ToListAsync();
        }
    }
}
