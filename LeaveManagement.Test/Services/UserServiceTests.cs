using AutoMapper;
using LeaveManagement.Application.DTO;
using LeaveManagement.Application.Services;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Infrastructure;
using LeaveManagement.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;
using DataModel = LeaveManagement.Infrastructure.DataModel;
using DTO = LeaveManagement.Application.DTO;
using Entity = LeaveManagement.Domain.Entities;

namespace LeaveManagement.Test.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly Mock<IManagerRepository> _mockManagerRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _userService;
        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _mockManagerRepository = new Mock<IManagerRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockUnitOfWork.Setup(e => e.User).Returns(_mockUserRepository.Object);
            _userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, 
            _mockEmployeeRepository.Object, _mockManagerRepository.Object);
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
        public async Task GetUserAsync_ShouldReturnEmptyList_WhenNoUserExist()
        {
            // Arrange
            var managers = new List<DataModel.UserData>();

            _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(managers);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
        {
            // Arrange
            var user = new DataModel.UserData(1, "John Doe", "john@gmail.com", "Test address 1", 5);

            var userDtos = new DTO.UserDTO { Id = 1, Name = "John Doe" };
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            _mockMapper.Setup(m=>m.Map<DTO.UserDTO>(user)).Returns(userDtos);

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }


        [Fact]
        public async Task GetUserByIdAsyncc_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((DataModel.UserData)null);

            // Act
            var result = await _userService.GetUserByIdAsync(999);

            // Assert
            Assert.Null(result);
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
        public async Task UpdateUserAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var userDto = new DTO.UserDTO { Id = 1, Name = "John Doe Updated" };

            _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Entity.User>())).ThrowsAsync(new KeyNotFoundException("Department not found"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _userService.UpdateUserAsync(userDto));
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


        [Fact]
        public async Task DeleteUserAsync_ShouldThrowException_WhenDeleteFails()
        {
            var userEntity = new Entity.User { Id = 1, Name = "John Doe" };

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<int>())).ReturnsAsync(userEntity);
            _mockUserRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Entity.User>())).ThrowsAsync(new Exception("Delete operation failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _userService.DeleteUserAsync(1));
        }

        [Fact]
        public async Task AddUserAsync_ShouldReturnAddedUser()
        {


            var User = new DTO.UserDTO
            {
                Id = 1,
                Name = "John Doe"
            };

            var UserEntity = new Entity.User
            {
                Name = "John Doe",
            };

            _mockMapper.Setup(m => m.Map<Entity.User>(User)).Returns(UserEntity);
            _mockUserRepository.Setup(repo => repo.AddAsync(UserEntity)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.UserDTO>(UserEntity)).Returns(User);

            // Act
            var result = await _userService.AddUserByAsync(User);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);

            _mockUnitOfWork.Verify(e => e.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddUserAsync_ShouldThrowException_WhenAddingUserFails()
        {
            // Arrange
            var User = new DTO.UserDTO
            {
                Id = 1,
                Name = "John Doe"
            };

            var UserEntity = new Entity.User
            {
                Name = "John Doe",
            };

            _mockMapper.Setup(m => m.Map<Entity.User>(User)).Returns(UserEntity);
            _mockUserRepository.Setup(repo => repo.AddAsync(UserEntity)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _userService.AddUserByAsync(User));
        }
        [Fact]
        public async Task AddUserAsync_ShouldThrowException_WhenUserAlreadyExists()
        {
            // Arrange
            var User = new DTO.UserDTO
            {
                Id = 1,
                Name = "John Doe"
            };

            var UserEntity = new Entity.User
            {
                Name = "John Doe",
            };

            _mockMapper.Setup(m => m.Map<Entity.User>(User)).Returns(UserEntity);
            _mockUserRepository.Setup(repo => repo.AddAsync(UserEntity)).ThrowsAsync(new InvalidOperationException("Department already exists"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.AddUserByAsync(User));
        }

        [Fact]
        public async Task AddUserAsync_WithEmployeeRole_ShouldAddEmployee()
        {
            // Arrange
            var userDto = new UserDTO
            {
                Id = 1,
                Name = "John Doe",
                Role = UserRole.Employee,
                ManagerId = 5
            };

            var userEntity = new User { Name = "John Doe" };

            _mockMapper.Setup(m => m.Map<User>(userDto)).Returns(userEntity);
            _mockUserRepository.Setup(r => r.AddAsync(userEntity)).Returns(Task.CompletedTask);
            _mockEmployeeRepository.Setup(r => r.AddAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<UserDTO>(userEntity)).Returns(userDto);

            // Act
            var result = await _userService.AddUserByAsync(userDto);

            // Assert
            Assert.Equal("John Doe", result.Name);
            _mockEmployeeRepository.Verify(r => r.AddAsync(It.Is<Employee>(e => e.ManagerId == 5)), Times.Once);
        }


        [Fact]
        public async Task AddUserAsync_WithManagerRole_ShouldAddManager()
        {
            // Arrange
            var userDto = new UserDTO
            {
                Id = 1,
                Name = "Jane Smith",
                Role = UserRole.Manager
            };

            var userEntity = new User { Name = "Jane Smith" };

            _mockMapper.Setup(m => m.Map<User>(userDto)).Returns(userEntity);
            _mockUserRepository.Setup(r => r.AddAsync(userEntity)).Returns(Task.CompletedTask);
            _mockManagerRepository.Setup(r => r.AddAsync(It.IsAny<Manager>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<UserDTO>(userEntity)).Returns(userDto);

            // Act
            var result = await _userService.AddUserByAsync(userDto);

            // Assert
            Assert.Equal("Jane Smith", result.Name);
            _mockManagerRepository.Verify(r => r.AddAsync(It.IsAny<Manager>()), Times.Once);
        }


        [Fact]
        public async Task AddUserAsync_ShouldSetDefaultPassword()
        {
            var userDto = new UserDTO { Name = "Default Pass", Role = UserRole.Employee };
            var userEntity = new User { Name = "Default Pass" };

            _mockMapper.Setup(m => m.Map<User>(userDto)).Returns(userEntity);
            _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>())).Callback<User>(u => userEntity = u).Returns(Task.CompletedTask);
            _mockEmployeeRepository.Setup(r => r.AddAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<User>())).Returns(userDto);

            // Act
            var result = await _userService.AddUserByAsync(userDto);

            // Assert
            Assert.Equal("experion@123", userEntity.Password);
        }



    }
}
