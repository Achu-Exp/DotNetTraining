using AutoMapper;
using LeaveManagement.Application.Services;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Infrastructure;
using LeaveManagement.Infrastructure.Repositories.Interfaces;
using DataModel = LeaveManagement.Infrastructure.DataModel;
using DTO = LeaveManagement.Application.DTO;
using Entity = LeaveManagement.Domain.Entities;
using Moq;
using Xunit;

namespace LeaveManagement.Test.Services
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IDepartmentRepository> _mockDepartmentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDepartmentRepository = new Mock<IDepartmentRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(x => x.Department).Returns(_mockDepartmentRepository.Object);
            _departmentService = new DepartmentService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetDepartmentAsync_ShouldReturnListOfDepartments()
        {
            // Arrange
            var departments = new List<DataModel.DepartmentData> {
                new DataModel.DepartmentData(1, "DU1", "Delivery Unit 1"),
                new DataModel.DepartmentData(2, "DU2", "Delivery Unit 2")
                };

            var departmentDto = new List<DTO.DepartmentDTO>
            {
                new DTO.DepartmentDTO
                {
                    Id = 1,
                    Name = "DU1",
                    Description = "Delivery Unit 1",
                },
                new DTO.DepartmentDTO
                {
                    Id=2,
                    Name = "DU2",
                    Description = "Delivery Unit 2"
                }
            };

            _mockDepartmentRepository.Setup(u => u.GetAllAsync()).ReturnsAsync(departments);
            _mockMapper.Setup(m=>m.Map<List<DTO.DepartmentDTO>>(departments)).Returns(departmentDto);

            // Act
            var result = await _departmentService.GetDepartmentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("DU1", result.FirstOrDefault()?.Name);
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_ShouldReturnDepartment_WhenDepartmentExists()
        {
            // Arrange
            var department = new DataModel.DepartmentData
                        (1, "DU1", "Delivery Unit 1");

            var departmentDto = new DTO.DepartmentDTO
            {
                Id = 1,
                Name = "DU1",
                Description = "Delivery Unit 1"
            };

            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(department);
            _mockMapper.Setup(m => m.Map<DTO.DepartmentDTO>(department)).Returns(departmentDto);

            // Act
            var result = await _departmentService.GetDepartmentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(departmentDto, result);
        }

        [Fact]
        public async Task AddDepartmentAsync_ShouldReturnAddedDepartment()
        {
            // Arrange
            var departmentDto = new DTO.DepartmentDTO
            {
                Id = 1,
                Name = "DU1",
                Description = "Delivery Unit 1"
            };

            var departmentEntity = new Entity.Department
            {
                Name = "DU1",
                Description = "Delivery Unit 1"
            };

            _mockMapper.Setup(m => m.Map<Entity.Department>(departmentDto)).Returns(departmentEntity);
            _mockDepartmentRepository.Setup(repo => repo.AddAsync(departmentEntity)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.DepartmentDTO>(departmentEntity)).Returns(departmentDto);

            // Act
            var result = await _departmentService.AddDepartmentByAsync(departmentDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task UpdateDepartmentAsync_ShouldReturnUpdatedDepartment()
        {
            // Arrange
            var departmentDto = new DTO.DepartmentDTO
            {
                Id = 1,
                Name = "DU1",
                Description = "Delivery Unit 1"
            };

            var departmentEntity = new Entity.Department
            {
                Name = "DU1",
                Description = "Delivery Unit 1"
            };

            _mockMapper.Setup(m => m.Map<Entity.Department>(departmentDto)).Returns(departmentEntity);
            _mockDepartmentRepository.Setup(repo => repo.AddAsync(departmentEntity)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.DepartmentDTO>(departmentEntity)).Returns(departmentDto);

            // Act
            var result = await _departmentService.UpdateDepartmentAsync(departmentDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("DU1", result.Name);
        }
    }
}
