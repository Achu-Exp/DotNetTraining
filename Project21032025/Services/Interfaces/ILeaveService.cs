using Project21032025.Models;

namespace Project21032025.Services.Interfaces
{
    public interface ILeaveService
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequests(); //done
        Task<LeaveRequest> GetLeaveRequestById(int id); //done
        Task ApplyForLeave(LeaveRequest leaveRequest); //done
        Task ApproveLeave(int id); //done
        Task RejectLeave(int id); //done
    }
}
