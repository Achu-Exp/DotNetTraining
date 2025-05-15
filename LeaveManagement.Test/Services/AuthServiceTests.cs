using Moq;
using Xunit;
using AutoMapper;
using LeaveManagement.Application.DTO;
using LeaveManagement.Domain.Entities;
using Microsoft.Extensions.Configuration;
using LeaveManagement.Application.Services;
using LeaveManagement.Infrastructure.DataModel;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Test.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IAuthRepository> _mockAuthRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockAuthRepository = new Mock<IAuthRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockMapper = new Mock<IMapper>();

            Environment.SetEnvironmentVariable("API_SECRET", "supersecretkey12345678901234567890");

            _authService = new AuthService(
                _mockAuthRepository.Object,
                _mockConfiguration.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task LoginUser_ValidCredentials_ShouldReturnTokenAndUser()
        {
            // Arrange
            var loginRequestDto = new LoginRequestDTO { Email = "john@example.com", Password = "password" };
            var loginRequestData = new LoginRequestData("john@example.com", "password");

            var user = new User { Id = 1, Email = "john@example.com", Password = "password" };
            var userDto = new UserDTO { Id = 1, Email = "john@example.com" };

            _mockMapper.Setup(m => m.Map<LoginRequestData>(loginRequestDto)).Returns(loginRequestData);
            _mockAuthRepository.Setup(a => a.LoginUser(loginRequestData)).ReturnsAsync((user, "Employee"));
            _mockMapper.Setup(m => m.Map<UserDTO>(user)).Returns(userDto);

            // Act
            var result = await _authService.LoginUser(loginRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
            Assert.Equal("john@example.com", result.User.Email);
        }

        [Fact]
        public async Task LoginUser_InvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var loginRequestDto = new LoginRequestDTO { Email = "wrong@example.com", Password = "wrongpass" };
            var loginRequestData = new LoginRequestData("john@example.com", "password");


            _mockMapper.Setup(m => m.Map<LoginRequestData>(loginRequestDto)).Returns(loginRequestData);
            _mockAuthRepository.Setup(a => a.LoginUser(loginRequestData)).ReturnsAsync((null, string.Empty));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _authService.LoginUser(loginRequestDto));
            Assert.Equal("Invalid email or password.", ex.Message);
        }

    }
}
