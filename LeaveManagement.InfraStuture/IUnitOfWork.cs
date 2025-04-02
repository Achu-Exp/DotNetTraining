using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        ILeaveRequestRepository LeaveRequests { get; }
        IEmployeeRepository Employee { get; }
        IManagerRepository Manager { get; }
        IUserRepository User { get; }
        IDepartmentRepository Department { get; }



        Task<int> CompleteAsync();
    }
}
