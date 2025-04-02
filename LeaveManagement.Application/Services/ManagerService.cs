using AutoMapper;
using LeaveManagement.Application.DTO;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Infrastructure;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Application.Services
{
    public class ManagerService : IManagerSevice
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IManagerRepository _managerRepository;
        private readonly IMapper _mapper;

        public ManagerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _managerRepository = _unitOfWork.Manager;
            _mapper = mapper;
        }
        public async Task<ManagerDTO> AddManagerByAsync(ManagerDTO manager)
        {
            var managerEntity = _mapper.Map<Entity.Manager>(manager);
            await _managerRepository.AddAsync(managerEntity);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<DTO.ManagerDTO>(managerEntity);
        }

        public async Task<int> DeleteManagerAsync(int id)
        {
            var manager = await _managerRepository.FindAsync(id);
            await _managerRepository.DeleteAsync(manager);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<ManagerDTO> GetManagerByIdAsync(int id)
        {
            var manager = await _managerRepository.GetByIdAsync(id);

            return _mapper.Map<DTO.ManagerDTO>(manager);
        }

        public async Task<List<ManagerDTO>> GetManagersAsync()
        {
            var entity = await _managerRepository.GetAllAsync();
            return _mapper.Map<List<ManagerDTO>>(entity);
        }

        public async Task<ManagerDTO> UpdateManagerAsync(ManagerDTO manager)
        {
            var managerEntity = _mapper.Map<Entity.Manager>(manager);
            await _managerRepository.UpdateAsync(managerEntity);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<DTO.ManagerDTO>(managerEntity);
        }
    }
}
