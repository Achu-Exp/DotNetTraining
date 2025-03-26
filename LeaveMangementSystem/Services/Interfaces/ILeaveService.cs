using LeaveMangementSystem.Models;
using LeaveMangementSystem.Models.DTO;

namespace LeaveMangementSystem.Services.Interfaces
{
    public interface ILeaveService
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequests();
        Task<LeaveRequest> GetLeaveRequestById(int id);
        Task ApplyForLeave(LeaveRequestDTO leaveRequestDto);
        Task ApproveLeave(int id);
        Task RejectLeave(int id);
    }
}
