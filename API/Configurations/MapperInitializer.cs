using API.Dtos;
using API.Models;
using AutoMapper;

namespace API.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<AppUser, AuthRequestDto>().ReverseMap();
        }
    }
}
