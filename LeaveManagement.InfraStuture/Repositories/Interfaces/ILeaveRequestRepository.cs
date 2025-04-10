using LeaveManagement.Infrastructure.DataModel;

namespace LeaveManagement.Infrastructure.Repositories.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<List<LeaveRequestData>> GetAllAsync(DateOnly? startDate = null, DateOnly? endDate = null,
            LeaveStatus? status = LeaveStatus.Pending, int? employeeId = null, int? approverId = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 100);
        Task<LeaveRequestData?> GetByIdAsync(int id);
        Task AddAsync(LeaveRequest leaveRequest);
        Task<LeaveRequest> UpdateAsync(LeaveRequest leaveRequest);
        Task<bool> DeleteAsync(LeaveRequest? leaveRequest);
        Task<LeaveRequest?> FindAsync(int id);
        Task<List<LeaveRequestData>> GetLeaveRequestByApprover(int id);
    }
}
