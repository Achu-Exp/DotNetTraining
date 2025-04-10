using AutoMapper;
using LeaveManagement.Application.Services;
using DTO = LeaveManagement.Application.DTO;
using LeaveManagement.Infrastructure;
using DataModel = LeaveManagement.Infrastructure.DataModel;
using LeaveManagement.Infrastructure.Repositories.Interfaces;
using Entity = LeaveManagement.Domain.Entities;

using Moq;
using Xunit;
using LeaveManagement.Application.Services.Interfaces;

namespace LeaveManagement.Test.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _userService;
        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockUnitOfWork.Setup(e => e.User).Returns(_mockUserRepository.Object);
            _userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnListOfUsers()
        {
            // Arrange
            var users = new List<DataModel.UserData>
            {
                new(1, "John Doe", "john@gmail.com", "Test address 1", 5),
                new(1, "Dasy", "dasy@gmail.com", "Test address 2", 1)
            };

            var userDtos = new List<DTO.UserDTO>
            {
                new DTO.UserDTO {Id = 1, Name = "John Doe"},
                new DTO.UserDTO {Id = 2, Name = "Dasy"}
            };
            _mockUserRepository.Setup(repo=>repo.GetAllAsync()).ReturnsAsync(users);
            _mockMapper.Setup(m=>m.Map<List<DTO.UserDTO>>(users)).Returns(userDtos);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John Doe", result.FirstOrDefault()?.Name);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
        {
            // Arrange
            var users = new DataModel.UserData(1, "John Doe", "john@gmail.com", "Test address 1", 5);

            var userDtos = new DTO.UserDTO { Id = 1, Name = "John Doe" };
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(users);
            _mockMapper.Setup(m=>m.Map<DTO.UserDTO>(users)).Returns(userDtos);

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnUpdatedUser()
        {
            // Arrange
            var userDto = new DTO.UserDTO { Id = 1, Name = "John Doe Updated" };
            var userEntity = new Entity.User { Id = 1, Name = "John Doe Updated" };

            _mockMapper.Setup(m => m.Map<Entity.User>(userDto)).Returns(userEntity);
            _mockUserRepository.Setup(repo => repo.UpdateAsync(userEntity)).Returns(Task.FromResult(userEntity));
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.UserDTO>(userEntity)).Returns(userDto);

            // Act
            var result = await _userService.UpdateUserAsync(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe Updated", result.Name);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldReturnNumberOfAffectedRows()
        {
            // Arrange
            var userEntity = new Entity.User { Id = 1, Name = "John Doe" };

            _mockUserRepository.Setup(repo => repo.FindAsync(1)).ReturnsAsync(userEntity);
            _mockUserRepository.Setup(repo => repo.DeleteAsync(userEntity)).Returns(Task.FromResult(true));
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _userService.DeleteUserAsync(1);

            // Assert
            Assert.Equal(1, result);
        }
    }
}
