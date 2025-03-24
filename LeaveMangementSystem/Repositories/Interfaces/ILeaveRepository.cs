using Project21032025.Models;

namespace Project21032025.Repositories.Interfaces
{
    public interface ILeaveRepository
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequests(); //done
        Task<LeaveRequest> GetLeaveRequestById(int userId); //done
        Task AddLeaveRequest(LeaveRequest request); //done
        Task UpdateLeaveRequest(LeaveRequest request); //done
        Task DeleteLeaveRequest(int userId); //done
    }
}
