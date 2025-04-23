using AutoMapper;
using LeaveManagement.Application.Services;
using DTO = LeaveManagement.Application.DTO;
using LeaveManagement.Infrastructure.Repositories.Interfaces;
using LeaveManagement.Infrastructure;
using DataModel = LeaveManagement.Infrastructure.DataModel;
using Entity = LeaveManagement.Domain.Entities;
using Moq;
using Xunit;

namespace LeaveManagement.Test.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(u => u.Employee).Returns(_mockEmployeeRepository.Object);
            _employeeService = new EmployeeService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetEmployeesAsync_ShouldReturnListOfEmployees()
        {
            // Arrange
            var employees = new List<DataModel.EmployeeData>
            {
                new (1,
                new DataModel.UserData(1, "John Doe", "john@example.com", "Kochi", 101),
                null
                ),
                new (2,
                new DataModel.UserData(2, "Aby Dan", "aby@example.com", "Trivandrum", 102),
                1)
            };

            var employeeDtos = new List<DTO.EmployeeDTO>
            {
                new DTO.EmployeeDTO
                {
                Id = 1,
                User = new DTO.UserDTO
                {
                   Id = 1,
                   Name = "John Doe",
                   Email = "john@example.com",
                   Address = "Kochi",
                   DepartmentId = 101
                },
                ManagerId = null
                },

                new DTO.EmployeeDTO
                {
                Id = 2,
                User = new DTO.UserDTO
                {
                   Id = 2,
                   Name = "Aby Dan",
                   Email = "aby@example.com",
                   Address = "Trivandrum",
                   DepartmentId = 102
                },
                ManagerId = 1
                }
            };

            _mockEmployeeRepository.Setup(repo => repo.GetAllAsync(null, null, null, true, 1, 100)).ReturnsAsync(employees);
            _mockMapper.Setup(m => m.Map<List<DTO.EmployeeDTO>>(employees)).Returns(employeeDtos);

            // Act
            var result = await _employeeService.GetEmployeesAsync(null, null, null, true, 1, 100);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John Doe", result.FirstOrDefault()?.User.Name);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employee = new DataModel.EmployeeData
                (1,
                new DataModel.UserData(1, "John Doe", "john@example.com", "Kochi", 101),
                null
                );
            var employeeDto = new DTO.EmployeeDTO
            {
                Id = 1,
                User = new DTO.UserDTO
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@example.com",
                    Address = "Kochi",
                    DepartmentId = 101
                },
                ManagerId = null
            };

            _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(employee);
            _mockMapper.Setup(m=>m.Map<DTO.EmployeeDTO>(employee)).Returns(employeeDto);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employeeDto, result);
        }

        [Fact]
        public async Task AddEmployeeAsync_ShouldReturnAddedEmployee()
        {
            // Arrange
            var employeeDto = new DTO.EmployeeDTO
            {
                Id = 1,
                User = new DTO.UserDTO
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@example.com",
                    Address = "Kochi",
                    DepartmentId = 101
                },
                ManagerId = null
            };
            var employeeEntity = new Entity.Employee
            {
                Id = 1,
                UserId = 1,
                ManagerId = null
            };

            _mockMapper.Setup(m => m.Map<Entity.Employee>(employeeDto)).Returns(employeeEntity);
            _mockEmployeeRepository.Setup(repo => repo.AddAsync(employeeEntity)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.EmployeeDTO>(employeeEntity)).Returns(employeeDto);

            // Act
            var result = await _employeeService.AddEmployeeByAsync(employeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldReturnUpdatedEmployee()
        {
            // Arrange
            var employeeDto = new DTO.EmployeeDTO
            {
                Id = 1,
                User = new DTO.UserDTO
                {
                    Id = 2,
                    Name = "John Doe updated",
                    Email = "john@example.com",
                    Address = "Kochi",
                    DepartmentId = 101
                },
                ManagerId = null
            };
            var employeeEntity = new Entity.Employee
            {
                Id = 1,
                UserId = 2,
                ManagerId = null
            };

            _mockMapper.Setup(m => m.Map<Entity.Employee>(employeeDto)).Returns(employeeEntity);
            _mockEmployeeRepository.Setup(repo => repo.UpdateAsync(employeeEntity)).ReturnsAsync(employeeEntity);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.EmployeeDTO>(employeeEntity)).Returns(employeeDto);

            // Act
            var result = await _employeeService.UpdateEmployeeAsync(employeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe updated", result.User.Name);
        }
    }
}
