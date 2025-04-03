using AutoMapper;
using LeaveManagement.Application.DTO;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Infrastructure;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Application.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
      
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILeaveRequestRepository _leaveRequestRepository;
            private readonly IMapper _mapper;

            public LeaveRequestService(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _leaveRequestRepository = _unitOfWork.LeaveRequests;
                _mapper = mapper;
            }
           

        public async Task<LeaveRequestDTO> GetLeaveRequestByIdAsync(int id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);

            return _mapper.Map<DTO.LeaveRequestDTO>(leaveRequest);
        }

        public async Task<List<LeaveRequestDTO>> GetLeaveRequestsAsync()
        {
           var entity = await _leaveRequestRepository.GetAllAsync();    
            return _mapper.Map<List<LeaveRequestDTO>>(entity);
        }

        //public async Task<LeaveRequestDTO> UpdateLeaveRequestAsync(LeaveRequestDTO leaveRequest)
        //{
        //    var leaveRequestEntity = _mapper.Map<Entity.LeaveRequest>(leaveRequest);
        //    await _leaveRequestRepository.UpdateAsync(leaveRequestEntity);
        //    await _unitOfWork.CompleteAsync();
        //    return _mapper.Map<DTO.LeaveRequestDTO>(leaveRequestEntity);
        //}
        public async Task<int> DeleteLeaveRequestAsync(int id)
        {
            var leaveRequest = await _leaveRequestRepository.FindAsync(id);
            await _leaveRequestRepository.DeleteAsync(leaveRequest);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<LeaveRequestDTO> AddLeaveRequestByAsync(LeaveRequestDTO leaveRequest)
        {
            var leaveRequestEntity = _mapper.Map<Entity.LeaveRequest>(leaveRequest);
            leaveRequestEntity.Status = LeaveStatus.Pending;
            await _leaveRequestRepository.AddAsync(leaveRequestEntity);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<DTO.LeaveRequestDTO>(leaveRequestEntity);
        }

        public async Task<bool> ApproveLeaveAsync(int id)
        {
            LeaveRequest leave = await _leaveRequestRepository.FindAsync(id);
            if (leave == null) return false;

            leave.Status = LeaveStatus.Approved;
            await _leaveRequestRepository.UpdateAsync(leave);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> RejectLeaveAsync(int id)
        {
            LeaveRequest leave = await _leaveRequestRepository.FindAsync(id);
            if (leave == null) return false;

            leave.Status = LeaveStatus.Rejected;
            await _leaveRequestRepository.UpdateAsync(leave);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
