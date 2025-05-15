using AutoMapper;
using LeaveManagement.Application.Services;
using LeaveManagement.Infrastructure;
using LeaveManagement.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;
using DataModel = LeaveManagement.Infrastructure.DataModel;
using DTO = LeaveManagement.Application.DTO;
using Entity = LeaveManagement.Domain.Entities;

namespace LeaveManagement.Test.Services
{
    public class ManagerServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IManagerRepository> _mockManagerRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ManagerService _managerService;

        public ManagerServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockManagerRepository = new Mock<IManagerRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(u => u.Manager).Returns(_mockManagerRepository.Object);
            _managerService = new ManagerService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetManagerAsync_ShouldReturnListOfManager()
        {
            // Arrange
            var managers = new List<DataModel.ManagerData> {
                new DataModel.ManagerData(1,
                new DataModel.UserData(1, "John", "john@example.com", "Kochi", 101)
                ),
                new DataModel.ManagerData(2,
                new DataModel.UserData(2, "James", "james@example.com", "Alpy", 102)
                )
                };

            var managerDto = new List<DTO.ManagerDTO>
            {
                new DTO.ManagerDTO
                {
                    Id = 1,
                    User = new DTO.UsersDTO
                    {
                        Id = 1,
                        Name = "John",
                        Email = "john@example.com",
                        Address = "Kochi",
                        DepartmentId = 101,
                    }
                },
                new DTO.ManagerDTO
                {
                    Id = 2,
                    User = new DTO.UsersDTO
                    {
                        Id = 1,
                        Name = "James",
                        Email = "james@example.com",
                        Address = "Alpy",
                        DepartmentId = 102,
                    }
                }
            };

            _mockManagerRepository.Setup(u => u.GetAllAsync()).ReturnsAsync(managers);
            _mockMapper.Setup(m => m.Map<List<DTO.ManagerDTO>>(managers)).Returns(managerDto);

            // Act
            var result = await _managerService.GetManagersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.User.Name == "James");
        }
        [Fact]
        public async Task GetManagerAsync_ShouldReturnEmptyList_WhenNoManagerExist()
        {
            // Arrange
            var managers = new List<DataModel.ManagerData>();  

            _mockManagerRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(managers);

            // Act
            var result = await _managerService.GetManagersAsync();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetManagerByIdAsync_ShouldReturnManager_WhenManagerExists()
        {
            // Arrange
            var manager = new DataModel.ManagerData(1,
                new DataModel.UserData(1, "John", "john@example.com", "Kochi", 101)
                );

            var managerDto = new DTO.ManagerDTO
            {
                Id = 1,
                User = new DTO.UsersDTO
                {
                    Id = 1,
                    Name = "John",
                    Email = "john@example.com",
                    Address = "Kochi",
                    DepartmentId = 101,
                }
            };

            _mockManagerRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(manager);
            _mockMapper.Setup(m => m.Map<DTO.ManagerDTO>(manager)).Returns(managerDto);

            // Act
            var result = await _managerService.GetManagerByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(managerDto, result);
        }

        [Fact]
        public async Task GetManagerByIdAsyncc_ShouldReturnNull_WhenManagerDoesNotExist()
        {
            // Arrange
            _mockManagerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((DataModel.ManagerData)null);

            // Act
            var result = await _managerService.GetManagerByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddManagerAsync_ShouldReturnAddedManager()
        {
            // Arrange
            var managerDto = new DTO.ManagerDTO
            {
                Id = 1,
                User = new DTO.UsersDTO
                {
                    Id = 1,
                    Name = "John",
                    Email = "john@example.com",
                    Address = "Kochi",
                    DepartmentId = 101,
                }
            };

            var managerEntity = new Entity.Manager
            {
                UserId = 1
            };

            _mockMapper.Setup(m => m.Map<Entity.Manager>(managerDto)).Returns(managerEntity);
            _mockManagerRepository.Setup(repo => repo.AddAsync(managerEntity)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.ManagerDTO>(managerEntity)).Returns(managerDto);

            // Act
            var result = await _managerService.AddManagerByAsync(managerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task AddManagerAsync_ShouldThrowException_WhenAddingManagerFails()
        {
            // Arrange
            var managerDto = new DTO.ManagerDTO
            {
                Id = 1,
                User = new DTO.UsersDTO
                {
                    Id = 1,
                    Name = "John",
                    Email = "john@example.com",
                    Address = "Kochi",
                    DepartmentId = 101,
                }
            };

            var managerEntity = new Entity.Manager
            {
                UserId = 1
            };

            _mockMapper.Setup(m => m.Map<Entity.Manager>(managerDto)).Returns(managerEntity);
            _mockManagerRepository.Setup(repo => repo.AddAsync(managerEntity)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _managerService.AddManagerByAsync(managerDto));
        }
        [Fact]
        public async Task AddManagerAsync_ShouldThrowException_WhenManagerAlreadyExists()
        {
            // Arrange
            var managerDto = new DTO.ManagerDTO
            {
                Id = 1,
                User = new DTO.UsersDTO
                {
                    Id = 1,
                    Name = "John",
                    Email = "john@example.com",
                    Address = "Kochi",
                    DepartmentId = 101,
                }
            };

            var managerEntity = new Entity.Manager
            {
                UserId = 1
            };

            _mockMapper.Setup(m => m.Map<Entity.Manager>(managerDto)).Returns(managerEntity);
            _mockManagerRepository.Setup(repo => repo.AddAsync(managerEntity)).ThrowsAsync(new InvalidOperationException("Department already exists"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _managerService.AddManagerByAsync(managerDto));
        }


        [Fact]
        public async Task AddManagerAsync_ShouldSetDefaultPassword_WhenUserIsProvided()
        {
            // Arrange
            var employeeDto = new DTO.ManagerDTO
            {
                Id = 1,
                User = new DTO.UsersDTO
                {
                    Id = 1,
                    Name = "Jane Doe",
                    Email = "jane@example.com",
                    Address = "Delhi",
                    DepartmentId = 102
                },
            };

            var userEntity = new Entity.User
            {
                Id = 1,
                Name = "Jane Doe",
                Email = "jane@example.com",
                Address = "Delhi",
                DepartmentId = 102
            };

            var employeeEntity = new Entity.Manager
            {
                Id = 1,
                User = userEntity
            };

            _mockMapper.Setup(m => m.Map<Entity.Manager>(employeeDto)).Returns(employeeEntity);
            _mockManagerRepository.Setup(repo => repo.AddAsync(It.IsAny<Entity.Manager>()))
                .Callback<Entity.Manager>(emp => Assert.Equal("experion@123", emp.User.Password))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.ManagerDTO>(employeeEntity)).Returns(employeeDto);

            // Act
            var result = await _managerService.AddManagerByAsync(employeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane Doe", result.User.Name);
        }


        [Fact]
        public async Task AddManagerAsync_ShouldWork_WhenUserIsNull()
        {
            // Arrange
            var employeeDto = new DTO.ManagerDTO
            {
                Id = 2,
                User = null,
            };

            var employeeEntity = new Entity.Manager
            {
                Id = 2,
                User = null,
            };

            _mockMapper.Setup(m => m.Map<Entity.Manager>(employeeDto)).Returns(employeeEntity);
            _mockManagerRepository.Setup(repo => repo.AddAsync(employeeEntity)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.ManagerDTO>(employeeEntity)).Returns(employeeDto);

            // Act
            var result = await _managerService.AddManagerByAsync(employeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.User);
        }

        [Fact]
        public async Task UpdateManagerAsync_ShouldReturnUpdatedManager()
        {
            // Arrange
            var managerDto = new DTO.ManagerDTO
            {
                Id = 1,
                User = new DTO.UsersDTO
                {
                    Id = 1,
                    Name = "John",
                    Email = "john@example.com",
                    Address = "Kochi",
                    DepartmentId = 101,
                }
            };

            var managerEntity = new Entity.Manager
            {
                UserId = 1
            };

            _mockMapper.Setup(m => m.Map<Entity.Manager>(managerDto)).Returns(managerEntity);
            _mockManagerRepository.Setup(repo => repo.UpdateAsync(managerEntity)).ReturnsAsync(managerEntity);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.ManagerDTO>(managerEntity)).Returns(managerDto);

            // Act
            var result = await _managerService.UpdateManagerAsync(managerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.User.Name);

            _mockManagerRepository.Verify(e => e.UpdateAsync(managerEntity), Times.Once);
            _mockUnitOfWork.Verify(e => e.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateManagerAsync_ShouldThrowException_WhenManagerNotFound()
        {
            // Arrange
            var managerDto = new DTO.ManagerDTO
            {
                Id = 1,
                User = new DTO.UsersDTO
                {
                    Id = 1,
                    Name = "John",
                    Email = "john@example.com",
                    Address = "Kochi",
                    DepartmentId = 101,
                }
            };
            _mockManagerRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Entity.Manager>())).ThrowsAsync(new KeyNotFoundException("Department not found"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _managerService.UpdateManagerAsync(managerDto));
        }

        [Fact]
        public async Task DeleteManagerAsync_ShouldReturnNoOfRowsAffected()
        {

            var manager = new Entity.Manager
            {
                UserId = 1
            };

            _mockManagerRepository.Setup(repo => repo.FindAsync(1)).ReturnsAsync(manager);
            _mockManagerRepository.Setup(r => r.DeleteAsync(manager)).Returns(Task.FromResult(true));
            _mockUnitOfWork.Setup(e => e.CompleteAsync()).ReturnsAsync(1);

            var result = await _managerService.DeleteManagerAsync(1);

            Assert.Equal(1, result);
            Assert.True(result > 0);

            _mockManagerRepository.Verify(repo => repo.FindAsync(1), Times.Once);
            _mockManagerRepository.Verify(repo => repo.DeleteAsync(manager), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteManagerAsync_ShouldThrowException_WhenDeleteFails()
        {
            // Arrange
            var manager = new Entity.Manager
            {
                UserId = 1
            };

            _mockManagerRepository.Setup(repo => repo.FindAsync(It.IsAny<int>())).ReturnsAsync(manager);
            _mockManagerRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Entity.Manager>())).ThrowsAsync(new Exception("Delete operation failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _managerService.DeleteManagerAsync(1));
        }
    }
}
