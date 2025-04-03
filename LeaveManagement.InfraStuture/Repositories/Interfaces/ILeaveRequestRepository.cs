using LeaveManagement.Infrastructure.DataModel;

namespace LeaveManagement.Infrastructure.Repositories.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<List<LeaveRequestData>> GetAllAsync();
        Task<LeaveRequestData?> GetByIdAsync(int id);
        Task AddAsync(LeaveRequest leaveRequest);
        Task<LeaveRequest> UpdateAsync(LeaveRequest leaveRequest);
        Task<bool> DeleteAsync(LeaveRequest? leaveRequest);
        Task<LeaveRequest?> FindAsync(int id);
        Task<List<LeaveRequestData>> GetLeaveRequestByApprover(int id);
    }
}
