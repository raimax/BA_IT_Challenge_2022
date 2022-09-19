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
            CreateMap<Book, BookResponseDto>().ReverseMap();
            CreateMap<ReservedBook, ReservedBookResponseDto>()
                .ForMember(dest => dest.ReservedBy, src => src.MapFrom(x => x.User.UserName))
                .ReverseMap();
            CreateMap<BorrowedBook, BorrowedBookResponseDto>()
                .ForMember(dest => dest.ReservedBy, src => src.MapFrom(x => x.User.UserName))
                .ReverseMap();
        }
    }
}
