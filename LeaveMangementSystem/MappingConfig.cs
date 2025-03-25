using AutoMapper;
using LeaveMangementSystem.Models;
using LeaveMangementSystem.Models.DTO;

namespace LeaveMangementSystem
{
    public class MappingConfig : Profile
    {
        public MappingConfig() {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
