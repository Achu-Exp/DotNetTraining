
using LeaveMangementSystem.Models;
using LeaveMangementSystem.Repositories.Interfaces;
using LeaveMangementSystem.Services.Interfaces;

namespace LeaveMangementSystem.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        public LeaveService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }
        public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequests()
        {
            return await _leaveRepository.GetAllLeaveRequests();
        }

        public async Task ApplyForLeave(LeaveRequest leaveRequest)
        {
            leaveRequest.Status = "Pending";
            await _leaveRepository.AddLeaveRequest(leaveRequest);
        }

        public async Task<LeaveRequest> GetLeaveRequestById(int id)
        {
            return await _leaveRepository.GetLeaveRequestById(id);
        }

        public async Task ApproveLeave(int id)
        {
            LeaveRequest leave = await _leaveRepository.GetLeaveRequestById(id);
            if (leave != null)
            {
                leave.Status = "Approved";
                await _leaveRepository.UpdateLeaveRequest(leave);
            }
        }

        public async Task RejectLeave(int id)
        {
            LeaveRequest leave = await _leaveRepository.GetLeaveRequestById(id);
            if (leave != null)
            {
                leave.Status = "Rejected";
                await _leaveRepository.UpdateLeaveRequest(leave);
            }
        }
    }
}