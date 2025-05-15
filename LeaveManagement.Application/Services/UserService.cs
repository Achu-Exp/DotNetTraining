using AutoMapper;
using LeaveManagement.Infrastructure;
using LeaveManagement.Application.DTO;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IManagerRepository _managerRepository;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, 
            IEmployeeRepository employeeRepository, IManagerRepository managerRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.User;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _managerRepository = managerRepository;
        }

        public async Task<UserDTO> AddUserByAsync(UserDTO userDto)
        {
            var userEntity = _mapper.Map<User>(userDto);
            userEntity.Password = "experion@123";

            await _userRepository.AddAsync(userEntity);
            await _unitOfWork.CompleteAsync();

            switch (userDto.Role)
            {
                case UserRole.Employee:
                    var employee = new Employee
                    {
                        UserId = userEntity.Id,
                        ManagerId = userDto.ManagerId
                    };
                    await _employeeRepository.AddAsync(employee);
                    await _unitOfWork.CompleteAsync();

                    var employeeDto = _mapper.Map<UserDTO>(userEntity);
                    employeeDto.Role = UserRole.Employee;
                    employeeDto.ManagerId = userDto.ManagerId;
                    return employeeDto;

                case UserRole.Manager:
                    var manager = new Manager
                    {
                        UserId = userEntity.Id
                    };
                    await _managerRepository.AddAsync(manager);
                    await _unitOfWork.CompleteAsync();

                    var managerDto = _mapper.Map<UserDTO>(userEntity);
                    managerDto.Role = UserRole.Manager;
                    return managerDto;

                default:
                    var defaultDto = _mapper.Map<UserDTO>(userEntity);
                    defaultDto.Role = UserRole.User;
                    return defaultDto;
            }
        }



        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
           var user = await _userRepository.GetByIdAsync(id);
           return _mapper.Map<DTO.UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
           var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<DTO.UserDTO>>(users);
        }

        public async Task<UserDTO> UpdateUserAsync(UserDTO user)
        {
            var userEntity = _mapper.Map<Entity.User> (user);
            await _userRepository.UpdateAsync(userEntity);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<DTO.UserDTO>(userEntity);

        }
        public async Task<int> DeleteUserAsync(int id)
        {
            var userentity = await _userRepository.FindAsync(id);
            await _userRepository.DeleteAsync(userentity);
            return await _unitOfWork.CompleteAsync();
        }
    }
}
