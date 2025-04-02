using LeaveManagement.Infrastructure.Repositories;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;

        public ILeaveRequestRepository LeaveRequests { get; }
        public IManagerRepository Manager { get; }
        public IEmployeeRepository Employee { get; }
        public IUserRepository User { get; }
        public IDepartmentRepository Department { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            LeaveRequests = new LeaveRequestRepository(context);
            Manager = new ManagerRepository(context);
            Employee = new EmployeeRepository(context);
            User = new UserRepository(context);
            Department = new DepartmentRepository(context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
