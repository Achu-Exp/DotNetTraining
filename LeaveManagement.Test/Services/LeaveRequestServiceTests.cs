using AutoMapper;
using LeaveManagement.Application.Services;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Infrastructure;
using DataModel = LeaveManagement.Infrastructure.DataModel;
using Entity = LeaveManagement.Domain.Entities;
using DTO = LeaveManagement.Application.DTO;
using LeaveManagement.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;
using LeaveManagement.Application.DTO;

namespace LeaveManagement.Test.Services
{
    public class LeaveRequestServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepository;
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IManagerRepository> _mockManagerRepository;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockEmailService = new Mock<IEmailService>();
            _mockMapper = new Mock<IMapper>();
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _mockManagerRepository = new Mock<IManagerRepository>();
            _mockLeaveRequestRepository = new Mock<ILeaveRequestRepository>();

            _mockUnitOfWork.Setup(u => u.LeaveRequests).Returns(_mockLeaveRequestRepository.Object);
            _leaveRequestService = new LeaveRequestService(_mockUnitOfWork.Object, _mockMapper.Object, 
                _mockManagerRepository.Object, _mockEmployeeRepository.Object, _mockEmailService.Object);
        }

        [Fact]
        public async Task GetLeaveRequestAsync_ShouldReturnListOfLeaveRequests()
        {
            var leaveRequests = new List<DataModel.LeaveRequestData>
            {
                new(
                    1,
                    new DateOnly(2025, 4, 15),
                    new DateOnly(2025, 4, 17),
                    "Medical leave",
                    LeaveStatus.Pending,
                    101,
                    201
                    ),
                new(
                    2,
                    new DateOnly(2025, 5, 1),
                    new DateOnly(2025, 5, 3),
                    "Personal work",
                    LeaveStatus.Approved,
                    102,
                    202
                    )
            };

            var leaveRequestDto = new List<DTO.LeaveRequestDTO>
            {
                new DTO.LeaveRequestDTO
                {
                    Id = 1,
                    StartDate = new DateOnly(2025, 4, 15),
                    EndDate = new DateOnly(2025, 4, 17),
                    Reason = "Medical Leave",
                    Status = LeaveStatus.Pending,
                    EmployeeId = 101,
                    ApproverId = 201
                },
                new DTO.LeaveRequestDTO
                {
                    Id = 2,
                    StartDate = new DateOnly(2025, 5, 1),
                    EndDate = new DateOnly(2025, 5, 3),
                    Reason = "Personal work",
                    Status = LeaveStatus.Approved,
                    EmployeeId = 102,
                    ApproverId = 202
                }
            };

            _mockLeaveRequestRepository.Setup(u => u.GetAllAsync(null, null, LeaveStatus.Pending, null, null, null, true, 1, 100)).ReturnsAsync(leaveRequests);
            _mockMapper.Setup(m => m.Map<List<DTO.LeaveRequestDTO>>(leaveRequests)).Returns(leaveRequestDto);

            var result = await _leaveRequestService.GetLeaveRequestsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async Task GetLeaveRequestByIdAsync_ShouldReturnLeaveRequest_WhenLeaveRequestExists()
        {
            // Arrange
            var leaveRequest = new DataModel.LeaveRequestData
                (
                    1,
                    new DateOnly(2025, 4, 15),
                    new DateOnly(2025, 4, 17),
                    "Medical leave",
                    LeaveStatus.Pending,
                    101,
                    201
                );

            var leaveRequestDto = new DTO.LeaveRequestDTO
            {
                Id = 1,
                StartDate = new DateOnly(2025, 4, 15),
                EndDate = new DateOnly(2025, 4, 17),
                Reason = "Medical Leave",
                Status = LeaveStatus.Pending,
                EmployeeId = 101,
                ApproverId = 201
            };

            _mockLeaveRequestRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(leaveRequest);
            _mockMapper.Setup(m => m.Map<DTO.LeaveRequestDTO>(leaveRequest)).Returns(leaveRequestDto);

            // Act
            var result = await _leaveRequestService.GetLeaveRequestByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, leaveRequestDto.Id);
        }

        [Fact]
        public async Task AddLeaveRequestAsync_ShouldReturnAddedLeaveRequest()
        {
            // Arrange
            var leaveRequestDto = new DTO.LeaveRequestDTO
            {
                Id = 1,
                StartDate = new DateOnly(2025, 4, 15),
                EndDate = new DateOnly(2025, 4, 17),
                Reason = "Medical Leave",
                Status = LeaveStatus.Pending,
                EmployeeId = 101,
                ApproverId = 201
            };

            var leaveRequestEntity = new Entity.LeaveRequest
            {
                Id = leaveRequestDto.Id,
                StartDate = leaveRequestDto.StartDate,
                EndDate = leaveRequestDto.EndDate,
                Reason = leaveRequestDto.Reason,
                Status = leaveRequestDto.Status,
                EmployeeId = leaveRequestDto.EmployeeId,
                ApproverId = leaveRequestDto.ApproverId
            };

            _mockMapper.Setup(m => m.Map<Entity.LeaveRequest>(leaveRequestDto)).Returns(leaveRequestEntity);
            _mockLeaveRequestRepository.Setup(repo => repo.AddAsync(leaveRequestEntity)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.LeaveRequestDTO>(leaveRequestEntity)).Returns(leaveRequestDto);

            // Act
            var result = await _leaveRequestService.AddLeaveRequestByAsync(leaveRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task ApproveLeaveRequestAsync_ShouldReturnTruet()
        {
            // Arrange
            var leaveRequestDto = new DTO.LeaveRequestDTO
            {
                Id = 1,
                StartDate = new DateOnly(2025, 4, 15),
                EndDate = new DateOnly(2025, 4, 17),
                Reason = "Medical Leave",
                Status = LeaveStatus.Pending,
                EmployeeId = 101,
                ApproverId = 201
            };

            var leaveRequestEntity = new Entity.LeaveRequest
            {
                Id = leaveRequestDto.Id,
                StartDate = leaveRequestDto.StartDate,
                EndDate = leaveRequestDto.EndDate,
                Reason = leaveRequestDto.Reason,
                Status = leaveRequestDto.Status,
                EmployeeId = leaveRequestDto.EmployeeId,
                ApproverId = leaveRequestDto.ApproverId
            };

            _mockLeaveRequestRepository.Setup(x => x.FindAsync(1)).ReturnsAsync(leaveRequestEntity);
            _mockLeaveRequestRepository.Setup(u => u.UpdateAsync(leaveRequestEntity)).ReturnsAsync(leaveRequestEntity);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            leaveRequestDto.Status = LeaveStatus.Approved;
            _mockMapper.Setup(m => m.Map<DTO.LeaveRequestDTO>(leaveRequestDto)).Returns(leaveRequestDto);

            var result = await _leaveRequestService.ApproveLeaveAsync(1);

            Assert.True(result);
        }

        public async Task RejectLeaveRequestAsync_ShouldReturnTrue()
        {
            // Arrange
            var leaveRequestDto = new DTO.LeaveRequestDTO
            {
                Id = 1,
                StartDate = new DateOnly(2025, 4, 15),
                EndDate = new DateOnly(2025, 4, 17),
                Reason = "Medical Leave",
                Status = LeaveStatus.Pending,
                EmployeeId = 101,
                ApproverId = 201
            };

            var leaveRequestEntity = new Entity.LeaveRequest
            {
                Id = leaveRequestDto.Id,
                StartDate = leaveRequestDto.StartDate,
                EndDate = leaveRequestDto.EndDate,
                Reason = leaveRequestDto.Reason,
                Status = leaveRequestDto.Status,
                EmployeeId = leaveRequestDto.EmployeeId,
                ApproverId = leaveRequestDto.ApproverId
            };

            _mockLeaveRequestRepository.Setup(x => x.FindAsync(1)).ReturnsAsync(leaveRequestEntity);
            _mockLeaveRequestRepository.Setup(u => u.UpdateAsync(leaveRequestEntity)).ReturnsAsync(leaveRequestEntity);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            leaveRequestDto.Status = LeaveStatus.Rejected;
            _mockMapper.Setup(m => m.Map<DTO.LeaveRequestDTO>(leaveRequestDto)).Returns(leaveRequestDto);

            var result = await _leaveRequestService.ApproveLeaveAsync(1);

            Assert.True(result);
        }
    }
}
