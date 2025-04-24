using AutoMapper;
using LeaveManagement.Application.DTO;
using LeaveManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        { 
            CreateMap<DTO.EmployeeDTO, Entity.Employee>().ReverseMap();
            CreateMap<EmployeeData, DTO.EmployeeDTO>().ReverseMap();
            CreateMap<EmployeeData, Entity.Employee>().ReverseMap();

            CreateMap<DTO.ManagerDTO, Entity.Manager>().ReverseMap();
            CreateMap<ManagerData, DTO.ManagerDTO>().ReverseMap();
            CreateMap<ManagerData, Entity.Manager>().ReverseMap();

            CreateMap<DTO.LeaveRequestDTO, Entity.LeaveRequest>().ReverseMap();
            CreateMap<LeaveRequestData, DTO.LeaveRequestDTO>().ReverseMap();
            CreateMap<LeaveRequestData, Entity.LeaveRequest>().ReverseMap();

            CreateMap<DTO.DepartmentDTO, Entity.Department>().ReverseMap();
            CreateMap<DepartmentData, DTO.DepartmentDTO>().ReverseMap();
            CreateMap<DepartmentData, Entity.Department>().ReverseMap();

            CreateMap<DTO.UserDTO, Entity.User>().ReverseMap();
            CreateMap<UserData, DTO.UserDTO>().ReverseMap();
            CreateMap<UserData, Entity.User>().ReverseMap();

            CreateMap<LoginRequestData, DTO.LoginRequestDTO>();
            CreateMap<DTO.LoginRequestDTO, LoginRequestData>().ReverseMap();
        }
    }
}
