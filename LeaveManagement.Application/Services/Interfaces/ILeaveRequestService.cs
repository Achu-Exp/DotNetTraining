namespace LeaveManagement.Application.Services.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<DTO.LeaveRequestDTO> AddLeaveRequestByAsync(DTO.LeaveRequestDTO leaveRequest);
        Task<DTO.LeaveRequestDTO> GetLeaveRequestByIdAsync(int id);
        Task<List<DTO.LeaveRequestDTO>> GetLeaveRequestsAsync();

        //Task<DTO.LeaveRequestDTO> UpdateLeaveRequestAsync(DTO.LeaveRequestDTO leaveRequest);
        Task<int> DeleteLeaveRequestAsync(int id);
        Task<List<DTO.LeaveRequestDTO>> GetLeaveRequestByApprover(int id);
        Task<bool> ApproveLeaveAsync(int id);
        Task<bool> RejectLeaveAsync(int id);
    }
}
