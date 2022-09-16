using API.Dtos;
using API.Helpers;

namespace API.IService
{
    public interface IBookService
    {
        Task<PagedList<BookResponseDto>> GetPagedListAsync(BookParams bookParams);
        Task<BookResponseDto> CreateAsync(BookRequestDto bookRequestDto);
        Task<BookResponseDto> FindByIdAsync(int id);
    }
}
