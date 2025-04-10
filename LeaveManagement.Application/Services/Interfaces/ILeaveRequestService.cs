using System.Globalization;
using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.Services.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<DTO.LeaveRequestDTO> AddLeaveRequestByAsync(DTO.LeaveRequestDTO leaveRequest);
        Task<DTO.LeaveRequestDTO> GetLeaveRequestByIdAsync(int id);
        Task<List<DTO.LeaveRequestDTO>> GetLeaveRequestsAsync(DateOnly? startDate = null, DateOnly? endDate = null, LeaveStatus? status = LeaveStatus.Pending, int? employeeId = null, int? approverId = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 100);

        //Task<DTO.LeaveRequestDTO> UpdateLeaveRequestAsync(DTO.LeaveRequestDTO leaveRequest);
        Task<int> DeleteLeaveRequestAsync(int id);
        Task<List<DTO.LeaveRequestDTO>> GetLeaveRequestByApprover(int id);
        Task<bool> ApproveLeaveAsync(int id);
        Task<bool> RejectLeaveAsync(int id);
    }
}
