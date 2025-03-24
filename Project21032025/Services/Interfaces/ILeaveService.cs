using Project21032025.Models;

namespace Project21032025.Services.Interfaces
{
    public interface ILeaveService
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequests();
        Task<LeaveRequest> GetLeaveRequestById(int id);
        Task ApplyForLeave(LeaveRequest leaveRequest);
        Task ApproveLeave(int id);
        Task RejectLeave(int id);
    }
}
