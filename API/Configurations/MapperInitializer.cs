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
            CreateMap<Author, AuthorResponseDto>().ReverseMap();
            CreateMap<Author, AuthorRequestDto>().ReverseMap();
            CreateMap<Publisher, PublisherResponseDto>().ReverseMap();
            CreateMap<Publisher, PublisherRequestDto>().ReverseMap();
            CreateMap<Status, StatusResponseDto>().ReverseMap();
            CreateMap<Book, BookRequestDto>().ReverseMap();
            CreateMap<Book, BookResponseDto>()
                //.ForMember(dest => dest.Author, src => src.MapFrom(src => src.Author))
                //.ForMember(dest => dest.Publisher, src => src.MapFrom(src => src.Publisher))
                //.ForMember(dest => dest.Status, src => src.MapFrom(src => src.Status))
                .ReverseMap();
        }
    }
}
