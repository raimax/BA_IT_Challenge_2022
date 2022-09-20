using API.Dtos;
using API.Helpers;

namespace API.IService
{
    public interface IBookService
    {
        Task<PagedList<BookResponseDto>> GetPagedListAsync(BookParams bookParams);
        Task<PagedList<ReservedBookResponseDto>> GetReservedPagedListAsync(PaginationParams paginationParams);
        Task<PagedList<BorrowedBookResponseDto>> GetBorrowedPagedListAsync(PaginationParams paginationParams);
        Task<BookResponseDto> CreateAsync(BookRequestDto bookRequestDto);
        Task<BookResponseDto> FindByIdAsync(int id);
        Task ReserveAsync(int bookId);
        Task BorrowAsync(int bookId);
        Task ReturnAsync(int bookId);
        Task DeleteAsync(int bookId);
    }
}
