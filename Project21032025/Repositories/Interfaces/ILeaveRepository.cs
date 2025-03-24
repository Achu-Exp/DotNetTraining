using Project21032025.Models;

namespace Project21032025.Repositories.Interfaces
{
    public interface ILeaveRepository
    {
        Task <IEnumerable<LeaveRequest>> GetAllLeaveRequests(); //done
        Task<LeaveRequest> GetLeaveRequest(int userId);
        Task AddLeaveRequest(LeaveRequest request);
        Task UpdateLeaveRequest(LeaveRequest request);
        Task DeleteLeaveRequest(int userId);
    }
}
