using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LeaveManagement.Application.Services;
using LeaveManagement.Infrastructure.Repositories.Interfaces;
using LeaveManagement.Infrastructure;
using DTO = LeaveManagement.Application.DTO;
using DataModel = LeaveManagement.Infrastructure.DataModel;
using Entity = LeaveManagement.Domain.Entities;
using Moq;
using Xunit;

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
                    User = new DTO.UserDTO
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
                    User = new DTO.UserDTO
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
        public async Task GetManagerByIdAsync_ShouldReturnManager_WhenManagerExists()
        {
            // Arrange
            var manager = new DataModel.ManagerData(1,
                new DataModel.UserData(1, "John", "john@example.com", "Kochi", 101)
                );

            var managerDto = new DTO.ManagerDTO
            {
                Id = 1,
                User = new DTO.UserDTO
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
        public async Task AddManagerAsync_ShouldReturnAddedManager()
        {
            // Arrange
            var managerDto = new DTO.ManagerDTO
            {
                Id = 1,
                User = new DTO.UserDTO
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
        public async Task UpdateManagerAsync_ShouldReturnUpdatedManager()
        {
            // Arrange
            var managerDto = new DTO.ManagerDTO
            {
                Id = 1,
                User = new DTO.UserDTO
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
            var result = await _managerService.UpdateManagerAsync(managerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.User.Name);
        }
    }
}
