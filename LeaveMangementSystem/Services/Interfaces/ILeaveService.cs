using LeaveMangementSystem.Models;
using LeaveMangementSystem.Models.DTO;

namespace LeaveMangementSystem.Services.Interfaces
{
    public interface ILeaveService
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequests(); //done
        Task<LeaveRequest> GetLeaveRequestById(int id); //done
        Task ApplyForLeave(LeaveRequestDTO leaveRequestDto); //done
        Task ApproveLeave(int id); //done
        Task RejectLeave(int id); //done
    }
}
