
using AutoMapper;
using LeaveMangementSystem.Models;
using LeaveMangementSystem.Models.DTO;
using LeaveMangementSystem.Repositories.Interfaces;
using LeaveMangementSystem.Services.Interfaces;
using Org.BouncyCastle.Asn1.Ocsp;

namespace LeaveMangementSystem.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IMapper _mapper;
        public LeaveService(ILeaveRepository leaveRepository, IMapper mapper)
        {
            _leaveRepository = leaveRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequests()
        {
            return await _leaveRepository.GetAllLeaveRequests();
        }

        public async Task ApplyForLeave(LeaveRequestDTO leaveRequestDto)
        {
            leaveRequestDto.Status = "Pending";
            var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestDto);
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