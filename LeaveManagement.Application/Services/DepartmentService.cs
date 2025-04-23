using AutoMapper;
using LeaveManagement.Application.DTO;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Infrastructure.Repositories.Interfaces;
using LeaveManagement.Infrastructure;
using LeaveManagement.Infrastructure.Repositories;
using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _departmentRepository = _unitOfWork.Department;
            _mapper = mapper;
        }
        public async Task<DepartmentDTO> AddDepartmentByAsync(DepartmentDTO department)
        {
            var departmentEntity = _mapper.Map<Entity.Department>(department);
            await _departmentRepository.AddAsync(departmentEntity);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<DTO.DepartmentDTO>(departmentEntity);    
        }

        public async Task<DepartmentDTO> GetDepartmentByIdAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);

            return _mapper.Map<DTO.DepartmentDTO>(department);
        }

        public async Task<List<DepartmentDTO>> GetDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();

            return _mapper.Map<List<DTO.DepartmentDTO>>(departments);
        }

        public async Task<DepartmentDTO> UpdateDepartmentAsync(DepartmentDTO department)
        {
            var departmentEntity = _mapper.Map<Entity.Department>(department);
            await _departmentRepository.UpdateAsync(departmentEntity);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<DTO.DepartmentDTO>(departmentEntity);
        }
        public async Task<int> DeleteDepartmentAsync(int id)
        {
            var department = await _departmentRepository.FindAsync(id);
            await _departmentRepository.DeleteAsync(department);
            return await _unitOfWork.CompleteAsync();
        }
    }
}
