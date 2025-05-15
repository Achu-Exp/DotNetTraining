using AutoMapper;
using LeaveManagement.Infrastructure;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Application.DTO;
using LeaveManagementSystem.Application.Services;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Application.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        //private readonly IEmailService _emailService;
        private readonly LeaveRequestNotifier _notifier;
        private readonly IMapper _mapper;

        public LeaveRequestService(IUnitOfWork unitOfWork, IMapper mapper,
            IEmployeeRepository employeeRepository, LeaveRequestNotifier notifier)
        {
            _unitOfWork = unitOfWork;
            _leaveRequestRepository = _unitOfWork.LeaveRequests;
            //_managerRepository = managerRepository;
            _employeeRepository = employeeRepository;
            //_emailService = emailService;
            _notifier = notifier;
            _mapper = mapper;
        }

        public async Task<LeaveRequestDTO> GetLeaveRequestByIdAsync(int id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            return _mapper.Map<DTO.LeaveRequestDTO>(leaveRequest);
        }
        
        public async Task<List<LeaveRequestDTO>> GetLeaveRequestsAsync(DateOnly? startDate = null,
            DateOnly? endDate = null, LeaveStatus? status = LeaveStatus.Pending, int? employeeId = null,
            int? approverId = null, string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 100)
        {
            var entity = await _leaveRequestRepository.GetAllAsync(startDate, endDate, status, employeeId,
                approverId, sortBy, isAscending, pageNumber, pageSize);
            return _mapper.Map<List<LeaveRequestDTO>>(entity);
        }

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

            // Raise the event
            _notifier.RaiseLeaveRequestCreated(leaveRequest.EmployeeId, leaveRequest.ApproverId);

            //var approver = await _managerRepository.FindAsync(leaveRequest.ApproverId);
            //var employee = await _employeeRepository.FindAsync(leaveRequest.EmployeeId);
            //if (approver != null)
            //{
            //    string subject = "New Leave Request Submitted";
            //    string body = $"Dear {approver.User.Name},<br/><br/>" +
            //                  $"A new leave request has been submitted by {employee.User.Name}.<br/>" +
            //                  $"Please review it at your earliest convenience.<br/><br/>" +
            //                  $"Best regards,<br/>Leave Management System";

            //    await _emailService.SendEmail(approver.User.Email, subject, body);
            //}

            return _mapper.Map<DTO.LeaveRequestDTO>(leaveRequestEntity);
        }

        public async Task<bool> ApproveLeaveAsync(int id)
        {
            LeaveRequest leave = await _leaveRequestRepository.FindAsync(id);
            if (leave == null) return false;

            leave.Status = LeaveStatus.Approved;
            await _leaveRequestRepository.UpdateAsync(leave);
            await _unitOfWork.CompleteAsync();

            var employee = await _employeeRepository.FindAsync(leave.EmployeeId);
            if (employee != null)
            {
                // Raise the event
                _notifier.RaiseLeaveStatusChanged(leave, employee.User);
            }

            //if (employee != null)
            //{
            //    string subject = "Your Leave Request has been Approved";
            //    string body = $"Dear {employee.User.Name},<br/><br/>" +
            //                  $"Your leave request from {leave.StartDate} to {leave.EndDate} has been approved.<br/>" +
            //                  $"Enjoy your time off!<br/><br/>" +
            //                  $"Best regards,<br/>Leave Management System";

            //    await _emailService.SendEmail(employee.User.Email, subject, body);
            //}
            return true;
        }

        public async Task<bool> RejectLeaveAsync(int id)
        {
            LeaveRequest leave = await _leaveRequestRepository.FindAsync(id);
            if (leave == null) return false;

            leave.Status = LeaveStatus.Rejected;
            await _leaveRequestRepository.UpdateAsync(leave);
            await _unitOfWork.CompleteAsync();

            var employee = await _employeeRepository.FindAsync(leave.EmployeeId);
            if (employee != null)
            {
                // Raise the event
                _notifier.RaiseLeaveStatusChanged(leave, employee.User);
            }

            //if (employee != null)
            //{
            //    string subject = "Your Leave Request has been Rejected";
            //    string body = $"Dear {employee.User.Name},<br/><br/>" +
            //                  $"Unfortunately, your leave request from {leave.StartDate} to {leave.EndDate} has been rejected.<br/>" +
            //                  $"If you need further information, please contact your manager.<br/><br/>" +
            //                  $"Best regards,<br/>Leave Management System";

            //    await _emailService.SendEmail(employee.User.Email, subject, body);
            //}
            return true;
        }

        public async Task<List<LeaveRequestDTO>> GetLeaveRequestByApprover(int id)
        {
            var leaveRequests = await _leaveRequestRepository.GetLeaveRequestByApprover(id);
            return _mapper.Map<List<LeaveRequestDTO>>(leaveRequests);
        }
    }
}
