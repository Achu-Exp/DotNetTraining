using Moq;
using Xunit;
using AutoMapper;
using LeaveManagement.Infrastructure;
using LeaveManagement.Application.Services;
using DTO = LeaveManagement.Application.DTO;
using Entity = LeaveManagement.Domain.Entities;
using DataModel = LeaveManagement.Infrastructure.DataModel;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

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
        public async Task GetDepartmentsAsync_ShouldReturnEmptyList_WhenNoDepartmentsExist()
        {
            // Arrange
            var departments = new List<DataModel.DepartmentData>();  // Empty list

            _mockDepartmentRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(departments);

            // Act
            var result = await _departmentService.GetDepartmentsAsync();

            // Assert
            Assert.Null(result);
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
        public async Task GetDepartmentByIdAsync_ShouldReturnNull_WhenDepartmentDoesNotExist()
        {
            // Arrange
            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((DataModel.DepartmentData)null);

            // Act
            var result = await _departmentService.GetDepartmentByIdAsync(999); 

            // Assert
            Assert.Null(result);  
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
        public async Task AddDepartmentAsync_ShouldThrowException_WhenAddingDepartmentFails()
        {
            // Arrange
            var departmentDto = new DTO.DepartmentDTO { Name = "DU1", Description = "Delivery Unit 1" };
            var departmentEntity = new Entity.Department { Name = "DU1", Description = "Delivery Unit 1" };

            _mockMapper.Setup(m => m.Map<Entity.Department>(departmentDto)).Returns(departmentEntity);
            _mockDepartmentRepository.Setup(repo => repo.AddAsync(departmentEntity)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _departmentService.AddDepartmentByAsync(departmentDto));
        }
        [Fact]
        public async Task AddDepartmentAsync_ShouldThrowException_WhenDepartmentAlreadyExists()
        {
            // Arrange
            var departmentDto = new DTO.DepartmentDTO { Name = "DU1", Description = "Delivery Unit 1" };
            var departmentEntity = new Entity.Department { Name = "DU1", Description = "Delivery Unit 1" };

            _mockMapper.Setup(m => m.Map<Entity.Department>(departmentDto)).Returns(departmentEntity);
            _mockDepartmentRepository.Setup(repo => repo.AddAsync(departmentEntity)).ThrowsAsync(new InvalidOperationException("Department already exists"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _departmentService.AddDepartmentByAsync(departmentDto));
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
            _mockDepartmentRepository.Setup(repo => repo.UpdateAsync(departmentEntity)).ReturnsAsync(departmentEntity);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.DepartmentDTO>(departmentEntity)).Returns(departmentDto);

            // Act
            var result = await _departmentService.UpdateDepartmentAsync(departmentDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("DU1", result.Name);

            _mockDepartmentRepository.Verify(e => e.UpdateAsync(departmentEntity), Times.Once);
            _mockUnitOfWork.Verify(e=>e.CompleteAsync(), Times.Once);
        }
        [Fact]
        public async Task UpdateDepartmentAsync_ShouldThrowException_WhenDepartmentNotFound()
        {
            // Arrange
            var departmentDto = new DTO.DepartmentDTO { Id = 999, Name = "DU1", Description = "Delivery Unit 1" }; 
            _mockDepartmentRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Entity.Department>())).ThrowsAsync(new KeyNotFoundException("Department not found"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _departmentService.UpdateDepartmentAsync(departmentDto));
        }

        [Fact]
        public async Task DeleteDepartmentAsync_ShouldReturnNumberOfAffectedRows()
        {
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

            _mockDepartmentRepository.Setup(repo => repo.FindAsync(1)).ReturnsAsync(departmentEntity);
            _mockDepartmentRepository.Setup(repo => repo.DeleteAsync(departmentEntity)).Returns(Task.FromResult(true));
            _mockUnitOfWork.Setup(e => e.CompleteAsync()).ReturnsAsync(1);

            var result = await _departmentService.DeleteDepartmentAsync(1);

            Assert.Equal(1, result);
            Assert.True(result > 0);

            _mockDepartmentRepository.Verify(repo => repo.FindAsync(1), Times.Once);
            _mockDepartmentRepository.Verify(repo => repo.DeleteAsync(departmentEntity), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);

        }
        [Fact]
        public async Task DeleteDepartmentAsync_ShouldThrowException_WhenDeleteFails()
        {
            // Arrange
            var departmentEntity = new Entity.Department { Id = 1, Name = "DU1", Description = "Delivery Unit 1" };

            _mockDepartmentRepository.Setup(repo => repo.FindAsync(It.IsAny<int>())).ReturnsAsync(departmentEntity);
            _mockDepartmentRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Entity.Department>())).ThrowsAsync(new Exception("Delete operation failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _departmentService.DeleteDepartmentAsync(1));
        }
    }
}
