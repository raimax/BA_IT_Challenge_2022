using API.Dtos;
using API.Extensions;
using API.Helpers;
using API.IService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<BookResponseDto>>> GetAll([FromQuery] BookParams bookParams)
        {
            PagedList<BookResponseDto> books = await _bookService.GetPagedListAsync(bookParams);

            Response.AddPaginationHeader(books.CurrentPage, books.PageSize, books.TotalCount, books.TotalPages);

            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult<BookResponseDto>> Create([FromBody] BookRequestDto bookRequestDto)
        {
            BookResponseDto bookResponseDto = await _bookService.CreateAsync(bookRequestDto);

            return Ok(bookResponseDto);
        }
    }
}
