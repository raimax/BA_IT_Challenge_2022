using API.Dtos;
using API.Extensions;
using API.Helpers;
using API.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{bookId}")]
        public async Task<ActionResult<BookResponseDto>> Get(int bookId)
        {
            return Ok(await _bookService.FindByIdAsync(bookId));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PagedList<BookResponseDto>>> GetAll([FromQuery] BookParams bookParams)
        {
            PagedList<BookResponseDto> books = await _bookService.GetPagedListAsync(bookParams);

            Response.AddPaginationHeader(books.CurrentPage, books.PageSize, books.TotalCount, books.TotalPages);

            return Ok(books);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<ActionResult<BookResponseDto>> Create([FromBody] BookRequestDto bookRequestDto)
        {
            BookResponseDto bookResponseDto = await _bookService.CreateAsync(bookRequestDto);

            return CreatedAtAction(nameof(Get), new { bookId = bookResponseDto.Id }, bookResponseDto);
        }

        [HttpPut("{bookId}/reserve")]
        public async Task<ActionResult> Reserve(int bookId)
        {
            await _bookService.ReserveAsync(bookId);

            return NoContent();
        }

        [HttpPut("{bookId}/borrow")]
        public async Task<ActionResult> Borrow(int bookId)
        {
            await _bookService.BorrowAsync(bookId);

            return NoContent();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("{bookId}/return")]
        public async Task<ActionResult> Return(int bookId)
        {
            await _bookService.ReturnAsync(bookId);

            return NoContent();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("reserved")]
        public async Task<ActionResult<PagedList<ReservedBookResponseDto>>> GetReserved([FromQuery] PaginationParams paginationParams)
        {
            PagedList<ReservedBookResponseDto> reservedBooks = await _bookService.GetReservedPagedListAsync(paginationParams);

            Response.AddPaginationHeader(reservedBooks.CurrentPage, reservedBooks.PageSize, reservedBooks.TotalCount, reservedBooks.TotalPages);

            return Ok(reservedBooks);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("borrowed")]
        public async Task<ActionResult<PagedList<BorrowedBookResponseDto>>> GetBorrowed([FromQuery] PaginationParams paginationParams)
        {
            PagedList<BorrowedBookResponseDto> borrowedBooks = await _bookService.GetBorrowedPagedListAsync(paginationParams);

            Response.AddPaginationHeader(borrowedBooks.CurrentPage, borrowedBooks.PageSize, borrowedBooks.TotalCount, borrowedBooks.TotalPages);

            return Ok(borrowedBooks);
        }
    }
}
