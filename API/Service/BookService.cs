using API.Data;
using API.Dtos;
using API.Exceptions;
using API.Helpers;
using API.IService;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Service
{
    public class BookService : AppService, IBookService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public BookService(DatabaseContext context, IMapper mapper, IHttpContextAccessor httpContext) : base(httpContext)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task BorrowAsync(int bookId)
        {
            AppUser? user = await _context.Users.SingleOrDefaultAsync(x => x.Id == CurrentUserId);
            Book? book = await _context.Books.SingleOrDefaultAsync(x => x.Id == bookId);

            if (user is null) throw new UnauthorizedException();
            if (book is null) throw new NotFoundException("Book not found");

            BorrowedBook borrowedBook = new()
            {
                BookId = bookId,
                UserId = user.Id,
            };

            await _context.BorrowedBooks.AddAsync(borrowedBook);
            book.StatusId = (int)BookStatus.BORROWED;

            if (await _context.SaveChangesAsync() < 1)
                throw new BadRequestException("Failed to borrow a book");
        }

        public async Task<BookResponseDto> CreateAsync(BookRequestDto bookRequestDto)
        {
            Book book = _mapper.Map<Book>(bookRequestDto);
            book.StatusId = (int)BookStatus.AVAILABLE;

            await _context.Books.AddAsync(book);

            if (await _context.SaveChangesAsync() < 1) throw new BadRequestException("Failed to create a book");

            return _mapper.Map<BookResponseDto>(book);
        }

        public async Task<BookResponseDto> FindByIdAsync(int id)
        {
            Book? book = await _context.Books
                .Include(x => x.Author)
                .Include(x => x.Publisher)
                .Include(x => x.Status)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (book is null) throw new NotFoundException("Book not found");

            return _mapper.Map<BookResponseDto>(book);
        }

        public async Task<PagedList<BookResponseDto>> GetPagedListAsync(BookParams bookParams)
        {
            IQueryable<BookResponseDto> books = _context.Books
                .ProjectTo<BookResponseDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(bookParams.Title))
            {
                books = books.Where(b => b.Title.ToLower().Contains(bookParams.Title.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(bookParams.Author))
            {
                string[] author = bookParams.Author.Split(' ');

                if (author.Length == 1)
                {
                    books = books.Where(b =>
                        b.Author.FirstName.ToLower() == author[0].ToLower() ||
                        b.Author.LastName.ToLower() == author[0].ToLower());
                }
                else if (author.Length == 2)
                {
                    books = books.Where(b => b.Author.FirstName.ToLower() == author[0].ToLower() && b.Author.LastName == author[1].ToLower());
                }
            }
            if (!string.IsNullOrWhiteSpace(bookParams.Publisher))
            {
                books = books.Where(b => b.Publisher.Name.ToLower().Contains(bookParams.Publisher.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(bookParams.PublishingDate))
            {
                try
                {
                    DateTime publishingDate = DateTime.Parse(bookParams.PublishingDate);
                    books = books.Where(b => b.PublishingDate.Date == publishingDate.Date);
                }
                catch (Exception)
                {

                }
            }
            if (!string.IsNullOrWhiteSpace(bookParams.Genre))
            {
                books = books.Where(b => b.Genre.ToLower().Contains(bookParams.Genre.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(bookParams.Isbn))
            {
                books = books.Where(b => b.Isbn.ToLower().Contains(bookParams.Isbn.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(bookParams.Status))
            {
                books = books.Where(b => b.Status.Name.ToLower() == bookParams.Status.ToLower());
            }

            return await PagedList<BookResponseDto>.CreateAsync(books, bookParams.PageNumber, bookParams.PageSize);
        }

        public async Task ReserveAsync(int bookId)
        {
            AppUser? user = await _context.Users.SingleOrDefaultAsync(x => x.Id == CurrentUserId);
            Book? book = await _context.Books.SingleOrDefaultAsync(x => x.Id == bookId);

            if (user is null) throw new UnauthorizedException();
            if (book is null) throw new NotFoundException("Book not found");

            ReservedBook reservedBook = new()
            {
                BookId = bookId,
                UserId = user.Id,
            };

            await _context.ReservedBooks.AddAsync(reservedBook);
            book.StatusId = (int)BookStatus.RESERVED;

            if (await _context.SaveChangesAsync() < 1)
                throw new BadRequestException("Failed to reserve a book");
        }
    }
}
