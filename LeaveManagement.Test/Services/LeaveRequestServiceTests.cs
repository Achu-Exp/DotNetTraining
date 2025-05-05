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
        public async Task GetLeaveRequestAsync_ShouldReturnEmptyList_WhenNoLeaveRequestExist()
        {
            // Arrange
            var employees = new List<DataModel.LeaveRequestData>();

            _mockLeaveRequestRepository
                .Setup(repo => repo.GetAllAsync(null, null, LeaveStatus.Pending, null, null, null, true, 1, 100))
                .ReturnsAsync(employees);
            // Act
            var result = await _leaveRequestService.GetLeaveRequestsAsync();

            // Assert
            Assert.Null(result);
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
        public async Task GetLeaveRequestByIdAsync_ShouldReturnNull_WhenLeaveRequestDoesNotExist()
        {
            // Arrange
            _mockLeaveRequestRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((DataModel.LeaveRequestData)null);

            // Act
            var result = await _leaveRequestService.GetLeaveRequestByIdAsync(999);

            // Assert
            Assert.Null(result);
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
        public async Task AddLeaveRequestAsync_ShouldThrowException_WhenAddingLeaveRequestFails()
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
            _mockLeaveRequestRepository.Setup(repo => repo.AddAsync(leaveRequestEntity)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _leaveRequestService.AddLeaveRequestByAsync(leaveRequestDto));
        }
        [Fact]
        public async Task AddLeaveRequestAsync_ShouldThrowException_WhenLeaveRequestAlreadyExists()
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
            _mockLeaveRequestRepository.Setup(repo => repo.AddAsync(leaveRequestEntity)).ThrowsAsync(new InvalidOperationException("Department already exists"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _leaveRequestService.AddLeaveRequestByAsync(leaveRequestDto));
        }

        [Fact]
        public async Task ApproveLeaveRequestAsync_ShouldReturnTrue()
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
            Assert.Equal(LeaveStatus.Approved, leaveRequestDto.Status);
        }

        [Fact]
        public async Task ApproveLeaveRequestAsync_LeaveNotFound_ShouldReturnFalse()
        {
            _mockLeaveRequestRepository.Setup(x => x.FindAsync(1)).ReturnsAsync((LeaveRequest)null);

            var result = await _leaveRequestService.ApproveLeaveAsync(1);

            Assert.False(result);
        }

        [Fact]
        public async Task ApproveLeaveRequestAsync_ShouldSendApprovalEmail()
        {
            var leaveRequest = new LeaveRequest
            {
                Id = 1,
                StartDate = new DateOnly(2025, 4, 15),
                EndDate = new DateOnly(2025, 4, 17),
                Reason = "Medical Leave",
                Status = LeaveStatus.Pending,
                EmployeeId = 101,
                ApproverId = 201
            };

            var employee = new Employee
            {
                Id = 101,
                User = new User { Name = "John", Email = "john@example.com" }
            };

            _mockLeaveRequestRepository.Setup(x => x.FindAsync(1)).ReturnsAsync(leaveRequest);
            _mockLeaveRequestRepository.Setup(x => x.UpdateAsync(It.IsAny<LeaveRequest>())).ReturnsAsync(leaveRequest);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockEmployeeRepository.Setup(e => e.FindAsync(101)).ReturnsAsync(employee);

            var result = await _leaveRequestService.ApproveLeaveAsync(1);

            Assert.True(result);
            _mockEmailService.Verify(e => e.SendEmail(
                "john@example.com",
                It.Is<string>(s => s.Contains("Approved")),
                It.Is<string>(b => b.Contains("Your leave request from"))
            ), Times.Once);
        }

        [Fact]
        public async Task ApproveLeaveRequestAsync_EmployeeNotFound_ShouldNotSendEmail()
        {
            var leaveRequest = new LeaveRequest
            {
                Id = 1,
                StartDate = new DateOnly(2025, 4, 15),
                EndDate = new DateOnly(2025, 4, 17),
                Reason = "Medical Leave",
                Status = LeaveStatus.Pending,
                EmployeeId = 101,
                ApproverId = 201
            };

            _mockLeaveRequestRepository.Setup(x => x.FindAsync(1)).ReturnsAsync(leaveRequest);
            _mockLeaveRequestRepository.Setup(x => x.UpdateAsync(It.IsAny<LeaveRequest>())).ReturnsAsync(leaveRequest);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockEmployeeRepository.Setup(e => e.FindAsync(101)).ReturnsAsync((Employee)null);

            var result = await _leaveRequestService.ApproveLeaveAsync(1);

            Assert.True(result);
            _mockEmailService.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
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

            var result = await _leaveRequestService.RejectLeaveAsync(1);

            Assert.True(result);
            Assert.Equal(LeaveStatus.Rejected, leaveRequestDto.Status);
        }

       

        [Fact]
        public async Task RejectLeaveRequestAsync_LeaveNotFound_ShouldReturnFalse()
        {
            _mockLeaveRequestRepository.Setup(x => x.FindAsync(1)).ReturnsAsync((LeaveRequest)null);

            var result = await _leaveRequestService.RejectLeaveAsync(1);

            Assert.False(result);
        }

       

        [Fact]
        public async Task RejectLeaveRequestAsync_ShouldSendRejectedMailEmail()
        {
            var leaveRequest = new LeaveRequest
            {
                Id = 1,
                StartDate = new DateOnly(2025, 4, 15),
                EndDate = new DateOnly(2025, 4, 17),
                Reason = "Medical Leave",
                Status = LeaveStatus.Pending,
                EmployeeId = 101,
                ApproverId = 201
            };

            var employee = new Employee
            {
                Id = 101,
                User = new User { Name = "John", Email = "john@example.com" }
            };

            _mockLeaveRequestRepository.Setup(x => x.FindAsync(1)).ReturnsAsync(leaveRequest);
            _mockLeaveRequestRepository.Setup(x => x.UpdateAsync(It.IsAny<LeaveRequest>())).ReturnsAsync(leaveRequest);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockEmployeeRepository.Setup(e => e.FindAsync(101)).ReturnsAsync(employee);

            var result = await _leaveRequestService.RejectLeaveAsync(1);

            Assert.True(result);
            _mockEmailService.Verify(e => e.SendEmail(
                "john@example.com",
                It.Is<string>(s => s.Contains("Rejected")),
                It.Is<string>(b => b.Contains("Unfortunately, your leave request from"))
            ), Times.Once);
        }

        [Fact]
        public async Task RejectLeaveRequestAsync_EmployeeNotFound_ShouldNotSendEmail()
        {
            var leaveRequest = new LeaveRequest
            {
                Id = 1,
                StartDate = new DateOnly(2025, 4, 15),
                EndDate = new DateOnly(2025, 4, 17),
                Reason = "Medical Leave",
                Status = LeaveStatus.Pending,
                EmployeeId = 101,
                ApproverId = 201
            };

            _mockLeaveRequestRepository.Setup(x => x.FindAsync(1)).ReturnsAsync(leaveRequest);
            _mockLeaveRequestRepository.Setup(x => x.UpdateAsync(It.IsAny<LeaveRequest>())).ReturnsAsync(leaveRequest);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockEmployeeRepository.Setup(e => e.FindAsync(101)).ReturnsAsync((Employee)null);

            var result = await _leaveRequestService.RejectLeaveAsync(1);

            Assert.True(result);
            _mockEmailService.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteLeaveRequestAsync_ShouldReturnNumberOfAffectedRows()
        {
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

            _mockLeaveRequestRepository.Setup(r=>r.FindAsync(1)).ReturnsAsync(leaveRequestEntity);
            _mockLeaveRequestRepository.Setup(r=>r.DeleteAsync(leaveRequestEntity)).Returns(Task.FromResult(true));
            _mockUnitOfWork.Setup(e=>e.CompleteAsync()).ReturnsAsync(1);

            var result = await _leaveRequestService.DeleteLeaveRequestAsync(1);

            Assert.Equal(1, result);
            Assert.True(result > 0);

            _mockLeaveRequestRepository.Verify(r => r.FindAsync(1), Times.Once);
            _mockLeaveRequestRepository.Verify(r => r.DeleteAsync(leaveRequestEntity), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteLeaveRequestAsync_ShouldThrowException_WhenDeleteFails()
        {
            var leaveRequest = new LeaveRequest
            {
                Id = 1,
                StartDate = new DateOnly(2025, 4, 15),
                EndDate = new DateOnly(2025, 4, 17),
                Reason = "Medical Leave",
                Status = LeaveStatus.Pending,
                EmployeeId = 101,
                ApproverId = 201
            };

            _mockLeaveRequestRepository.Setup(repo => repo.FindAsync(It.IsAny<int>())).ReturnsAsync(leaveRequest);
            _mockLeaveRequestRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Entity.LeaveRequest>())).ThrowsAsync(new Exception("Delete operation failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _leaveRequestService.DeleteLeaveRequestAsync(1));
        }



        [Fact]
        public async Task GetLeaveRequestByApprover_InputApproverId_ShouldReturnListOfLeaveRequests()
        {
            // Arrange
            int approverId = 201;

            var leaveRequests = new List<DataModel.LeaveRequestData>
            {
                new DataModel.LeaveRequestData(
                    1,
                    new DateOnly(2025, 4, 15),
                    new DateOnly(2025, 4, 17),
                    "Medical Leave",
                    LeaveStatus.Pending,
                    101,
                    approverId
                ),
                new DataModel.LeaveRequestData(
                    2,
                    new DateOnly(2025, 5, 1),
                    new DateOnly(2025, 5, 3),
                    "Family Function",
                    LeaveStatus.Pending,
                    102,
                    approverId
                )
            };

            var leaveRequestDtos = new List<DTO.LeaveRequestDTO>
            {
                new DTO.LeaveRequestDTO
                {
                    Id = 1,
                    StartDate = new DateOnly(2025, 4, 15),
                    EndDate = new DateOnly(2025, 4, 17),
                    Reason = "Medical Leave",
                    Status = LeaveStatus.Pending,
                    EmployeeId = 101,
                    ApproverId = approverId
                },
                new DTO.LeaveRequestDTO
                {
                    Id = 2,
                    StartDate = new DateOnly(2025, 5, 1),
                    EndDate = new DateOnly(2025, 5, 3),
                    Reason = "Family Function",
                    Status = LeaveStatus.Pending,
                    EmployeeId = 102,
                    ApproverId = approverId
                }
            };

            _mockLeaveRequestRepository
                .Setup(r => r.GetLeaveRequestByApprover(approverId))
                .ReturnsAsync(leaveRequests);

            _mockMapper
                .Setup(m => m.Map<List<DTO.LeaveRequestDTO>>(leaveRequests))
                .Returns(leaveRequestDtos);

            // Act
            var result = await _leaveRequestService.GetLeaveRequestByApprover(approverId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(approverId, r.ApproverId));
            Assert.Collection(result,
                item => Assert.Equal("Medical Leave", item.Reason),
                item => Assert.Equal("Family Function", item.Reason)
            );
        }

        [Fact]
        public async Task GetLeaveRequestByApprover_NoRequests_ShouldReturnEmptyList()
        {
            int approverId = 999;

            _mockLeaveRequestRepository
                .Setup(r => r.GetLeaveRequestByApprover(approverId))
                .ReturnsAsync(new List<DataModel.LeaveRequestData>());

            _mockMapper
                .Setup(m => m.Map<List<DTO.LeaveRequestDTO>>(It.IsAny<List<DataModel.LeaveRequestData>>()))
                .Returns(new List<DTO.LeaveRequestDTO>());

            var result = await _leaveRequestService.GetLeaveRequestByApprover(approverId);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}


