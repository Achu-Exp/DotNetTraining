using AutoMapper;
using LeaveManagement.Application.DTO;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Infrastructure;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.User;
            _mapper = mapper;
        }
        public async Task<UserDTO> AddUserByAsync(UserDTO user)
        {
            var userEntity = _mapper.Map<Entity.User>(user);
            await _userRepository.AddAsync(userEntity);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<UserDTO>(userEntity);
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
