
using Project21032025.Models;
using Project21032025.Repositories.Interfaces;

namespace Project21032025.Services
{
    public class LeaveService
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
    }
}
