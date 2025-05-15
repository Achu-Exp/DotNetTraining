using Moq;
using Xunit;
using AutoMapper;
using LeaveManagement.Infrastructure;
using LeaveManagement.Application.DTO;
using LeaveManagement.Application.Services;
using DTO = LeaveManagement.Application.DTO;
using Entity = LeaveManagement.Domain.Entities;
using LeaveManagement.Infrastructure.DataModel;
using DataModel = LeaveManagement.Infrastructure.DataModel;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

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
        public async Task GetEmployeesAsync_ShouldReturnEmptyList_WhenNoEmployeesExist()
        {
            // Arrange
            var employees = new List<DataModel.EmployeeData>();

            _mockEmployeeRepository
                .Setup(repo => repo.GetAllAsync(null, null, null, true, 1, 1000))
                .ReturnsAsync(employees);
            // Act
            var result = await _employeeService.GetEmployeesAsync();

            // Assert
            Assert.Null(result);
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
            _mockMapper.Setup(m => m.Map<DTO.EmployeeDTO>(employee)).Returns(employeeDto);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employeeDto, result);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnNull_WhenEmployeeDoesNotExist()
        {
            // Arrange
            _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((DataModel.EmployeeData)null);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(999);

            // Assert
            Assert.Null(result);
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
        public async Task AddEmployeeAsync_ShouldThrowException_WhenAddingEmployeeFails()
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
            _mockEmployeeRepository.Setup(repo => repo.AddAsync(employeeEntity)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _employeeService.AddEmployeeByAsync(employeeDto));
        }
        [Fact]
        public async Task AddEmployeeAsync_ShouldThrowException_WhenEmployeeAlreadyExists()
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
            _mockEmployeeRepository.Setup(repo => repo.AddAsync(employeeEntity)).ThrowsAsync(new InvalidOperationException("Department already exists"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _employeeService.AddEmployeeByAsync(employeeDto));
        }


        [Fact]
        public async Task AddEmployeeAsync_ShouldSetDefaultPassword_WhenUserIsProvided()
        {
            // Arrange
            var employeeDto = new DTO.EmployeeDTO
            {
                Id = 1,
                User = new DTO.UserDTO
                {
                    Id = 1,
                    Name = "Jane Doe",
                    Email = "jane@example.com",
                    Address = "Delhi",
                    DepartmentId = 102
                },
                ManagerId = null
            };

            var userEntity = new Entity.User
            {
                Id = 1,
                Name = "Jane Doe",
                Email = "jane@example.com",
                Address = "Delhi",
                DepartmentId = 102
            };

            var employeeEntity = new Entity.Employee
            {
                Id = 1,
                User = userEntity,
                ManagerId = null
            };

            _mockMapper.Setup(m => m.Map<Entity.Employee>(employeeDto)).Returns(employeeEntity);
            _mockEmployeeRepository.Setup(repo => repo.AddAsync(It.IsAny<Entity.Employee>()))
                .Callback<Entity.Employee>(emp => Assert.Equal("experion@123", emp.User.Password))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.EmployeeDTO>(employeeEntity)).Returns(employeeDto);

            // Act
            var result = await _employeeService.AddEmployeeByAsync(employeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane Doe", result.User.Name);
        }


        [Fact]
        public async Task AddEmployeeAsync_ShouldWork_WhenUserIsNull()
        {
            // Arrange
            var employeeDto = new DTO.EmployeeDTO
            {
                Id = 2,
                User = null,
                ManagerId = 3
            };

            var employeeEntity = new Entity.Employee
            {
                Id = 2,
                User = null,
                ManagerId = 3
            };

            _mockMapper.Setup(m => m.Map<Entity.Employee>(employeeDto)).Returns(employeeEntity);
            _mockEmployeeRepository.Setup(repo => repo.AddAsync(employeeEntity)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<DTO.EmployeeDTO>(employeeEntity)).Returns(employeeDto);

            // Act
            var result = await _employeeService.AddEmployeeByAsync(employeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.User);
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

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldThrowException_WhenEmployeeNotFound()
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
           
            _mockEmployeeRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Entity.Employee>())).ThrowsAsync(new KeyNotFoundException("Department not found"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _employeeService.UpdateEmployeeAsync(employeeDto));
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldReturnNumberOfAffectedRows()
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

            _mockEmployeeRepository.Setup(repo => repo.FindAsync(1)).ReturnsAsync(employeeEntity);
            _mockEmployeeRepository.Setup(repo => repo.DeleteAsync(employeeEntity)).Returns(Task.FromResult(true));
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _employeeService.DeleteEmployeeAsync(1);

            // Assert
            Assert.Equal(1, result);
        }
        [Fact]
        public async Task DeleteEmployeeAsync_ShouldThrowException_WhenDeleteFails()
        {
            // Arrange
            var employeeEntity = new Entity.Employee
            {
                Id = 1,
                UserId = 1,
                ManagerId = null
            };
            _mockEmployeeRepository.Setup(repo => repo.FindAsync(It.IsAny<int>())).ReturnsAsync(employeeEntity);
            _mockEmployeeRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Entity.Employee>())).ThrowsAsync(new Exception("Delete operation failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _employeeService.DeleteEmployeeAsync(1));
        }

        [Fact]
        public async Task GetEmployeeByDepartmentId_WithSingleEmployee_ReturnsCorrectEmployee()
        {
            // Arrange
            var departmentId = 101;

            var employees = new List<EmployeeData>
             {
                new EmployeeData(1,
                    new UserData(1, "John Doe", "john@example.com", "Kochi", departmentId),
                    null
                )
            };

            var employeeDtos = new List<EmployeeDTO>
            {
                new EmployeeDTO
                {
                    Id = 1,
                    User = new UserDTO
                    {
                        Id = 1,
                        Name = "John Doe",
                        Email = "john@example.com",
                        Address = "Kochi",
                        DepartmentId = departmentId
                    },
                    ManagerId = null
                }
            };

            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeByDepartmentId(departmentId)).ReturnsAsync(employees);
            _mockMapper.Setup(m => m.Map<List<EmployeeDTO>>(employees)).Returns(employeeDtos);

            // Act
            var result = await _employeeService.GetEmployeeByDepartmentId(departmentId);

            // Assert
            Assert.Single(result);
            Assert.Equal("John Doe", result[0].User.Name);
            Assert.Equal(departmentId, result[0].User.DepartmentId);
            _mockEmployeeRepository.Verify(repo => repo.GetEmployeeByDepartmentId(departmentId), Times.Once);
        }


        [Fact]
        public async Task GetEmployeeByDepartmentId_WithNoEmployees_ReturnsEmptyList()
        {
            // Arrange
            var departmentId = 101;
            var employees = new List<EmployeeData>();

            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeByDepartmentId(departmentId)).ReturnsAsync(employees);
            _mockMapper.Setup(m => m.Map<List<EmployeeDTO>>(employees)).Returns(new List<EmployeeDTO>());

            // Act
            var result = await _employeeService.GetEmployeeByDepartmentId(departmentId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockEmployeeRepository.Verify(repo => repo.GetEmployeeByDepartmentId(departmentId), Times.Once);
        }


        [Fact]
        public async Task GetAllEmployeessByManagerId_InputManagerIdInt_ShouldReturnListOfEmployess()
        {
            var managerId = 1;
            var employees = new List<EmployeeData>
            {
                new EmployeeData(1,
                    new UserData(1, "John Doe", "john@example.com", "Kochi", 101),
                    1
                ),
                new EmployeeData(2,
                    new UserData(2, "Aby Dan", "aby@example.com", "Trivandrum", 101),
                    1
                )
            };

            var employeeDtos = new List<EmployeeDTO>
            {
                new EmployeeDTO
                {
                    Id = 1,
                    User = new UserDTO
                    {
                        Id = 1,
                        Name = "John Doe",
                        Email = "john@example.com",
                        Address = "Kochi",
                        DepartmentId = 101
                    },
                    ManagerId = 1
                },
                new EmployeeDTO
                {
                    Id = 2,
                    User = new UserDTO
                    {
                        Id = 2,
                        Name = "Aby Dan",
                        Email = "aby@example.com",
                        Address = "Trivandrum",
                        DepartmentId = 101
                    },
                    ManagerId = 1
                }
            };

            _mockEmployeeRepository.Setup(repo => repo.GetAllEmployeesByManagerId(managerId)).ReturnsAsync(employees);
            _mockMapper.Setup(m => m.Map<List<EmployeeDTO>>(employees)).Returns(employeeDtos);

            // Act
            var result = await _employeeService.GetAllEmployeesByManagerId(managerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John Doe", result[0].User.Name);
            Assert.Equal("Aby Dan", result[1].User.Name);
            Assert.All(result, e => Assert.Equal(managerId, e.ManagerId)); 
            _mockEmployeeRepository.Verify(repo => repo.GetAllEmployeesByManagerId(managerId), Times.Once);
        }


        [Fact]
        public async Task GetAllEmployeesByManagerId_WithNoEmployees_ReturnsEmptyList()
        {
            // Arrange
            var managerId = 1;
            var employees = new List<EmployeeData>();

            _mockEmployeeRepository.Setup(repo => repo.GetAllEmployeesByManagerId(managerId)).ReturnsAsync(employees);
            _mockMapper.Setup(m => m.Map<List<EmployeeDTO>>(employees)).Returns(new List<EmployeeDTO>());

            // Act
            var result = await _employeeService.GetAllEmployeesByManagerId(managerId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockEmployeeRepository.Verify(repo => repo.GetAllEmployeesByManagerId(managerId), Times.Once);
        }


    }
}
