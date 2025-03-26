using LeaveMangementSystem.Models;
using LeaveMangementSystem.Models.DTO;

namespace LeaveMangementSystem.Repositories.Interfaces
{
    public interface ILeaveRepository
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequests();
        Task<LeaveRequest> GetLeaveRequestById(int userId);
        Task AddLeaveRequest(LeaveRequest request); 
        Task UpdateLeaveRequest(LeaveRequest request);
        Task DeleteLeaveRequest(int userId);
    }
}
