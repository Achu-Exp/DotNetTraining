using AutoMapper;
using Project21032025.Models;
using Project21032025.Models.DTO;

namespace Project21032025
{
    public class MappingConfig : Profile
    {
        public MappingConfig() {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
