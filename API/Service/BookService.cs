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
            AppUser? user = await _context.Users
                .Include(x => x.BorrowedBooks)
                .SingleOrDefaultAsync(x => x.Id == CurrentUserId);
            Book? book = await _context.Books
                .Include(x => x.Status)
                .SingleOrDefaultAsync(x => x.Id == bookId);
            BorrowedBook? existingBorrowedBook = await _context.BorrowedBooks
                .SingleOrDefaultAsync(x => x.BookId == bookId);

            if (user is null) throw new UnauthorizedException();
            if (book is null) throw new NotFoundException("Book not found");
            if (existingBorrowedBook?.UserId == CurrentUserId) throw new BadRequestException("You already borrowed this book");
            if (existingBorrowedBook is not null) throw new BadRequestException("Book is already borrowed");

            if (book.Status.Id == (int)BookStatus.RESERVED)
            {
                ReservedBook? reservedBook = await _context.ReservedBooks.SingleAsync(x => x.BookId == bookId);

                // only the user that reserved the book can borrow it
                if (reservedBook.UserId != CurrentUserId)
                    throw new BadRequestException("You cannot borrow a book that is reserved by someone else");

                _context.ReservedBooks.Remove(reservedBook);
            }

            BorrowedBook borrowedBook = new()
            {
                BookId = bookId,
                UserId = user.Id,
            };

            user.BorrowedBooks!.Add(borrowedBook);
            book.StatusId = (int)BookStatus.BORROWED;

            if (await _context.SaveChangesAsync() < 1)
                throw new BadRequestException("Failed to borrow a book");
        }

        public async Task<BookResponseDto> CreateAsync(BookRequestDto bookRequestDto)
        {
            Author? author = await _context.Authors
                .SingleOrDefaultAsync(x =>
                    x.FirstName.ToLower() == bookRequestDto.Author.FirstName.ToLower() &&
                    x.LastName.ToLower() == bookRequestDto.Author.LastName.ToLower());

            Publisher? publisher = await _context.Publishers
                .SingleOrDefaultAsync(x => x.Name.ToLower() == bookRequestDto.Publisher.Name.ToLower());

            Book book = _mapper.Map<Book>(bookRequestDto);
            book.StatusId = (int)BookStatus.AVAILABLE;
            if (author is not null) book.Author = author;
            if (publisher is not null) book.Publisher = publisher;

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
                    throw new BadRequestException("Can't get books: date format is invalid");
                }
            }
            if (!string.IsNullOrWhiteSpace(bookParams.Genre))
            {
                books = books.Where(b => b.Genre.ToLower().Contains(bookParams.Genre.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(bookParams.Isbn))
            {
                books = books.Where(b => b.Isbn.ToLower() == bookParams.Isbn.ToLower());
            }
            if (bookParams.Status >= 1 && bookParams.Status <= 3)
            {
                books = books.Where(b => b.Status.Id == bookParams.Status);
            }

            return await PagedList<BookResponseDto>.CreateAsync(books, bookParams.PageNumber, bookParams.PageSize);
        }

        public async Task ReserveAsync(int bookId)
        {
            AppUser? user = await _context.Users
                .Include(x => x.ReservedBooks)
                .SingleOrDefaultAsync(x => x.Id == CurrentUserId);
            Book? book = await _context.Books
                .Include(x => x.Status)
                .SingleOrDefaultAsync(x => x.Id == bookId);

            if (user is null) throw new UnauthorizedException();
            if (book is null) throw new NotFoundException("Book not found");
            if (user.ReservedBooks!.SingleOrDefault(x => x.BookId == bookId) is not null)
                throw new BadRequestException("You already reserved this book");
            if (book.Status.Id == (int)BookStatus.RESERVED)
                throw new BadRequestException("Book is already reserved");
            if (book.Status.Id == (int)BookStatus.BORROWED)
                throw new BadRequestException("You cannot reserve a borrowed book");

            ReservedBook reservedBook = new()
            {
                BookId = bookId,
                UserId = user.Id,
            };

            user.ReservedBooks!.Add(reservedBook);
            book.StatusId = (int)BookStatus.RESERVED;

            if (await _context.SaveChangesAsync() < 1)
                throw new BadRequestException("Failed to reserve a book");
        }

        public async Task ReturnAsync(int bookId)
        {
            Book? book = await _context.Books
                .Include(x => x.Status)
                .SingleOrDefaultAsync(x => x.Id == bookId);

            if (book is null) throw new NotFoundException("Book not found");

            switch (book.Status.Id)
            {
                case (int)BookStatus.RESERVED:
                    ReservedBook? reservedBook = await _context.ReservedBooks
                        .SingleOrDefaultAsync(x => x.BookId == bookId);

                    if (reservedBook is null) throw new NotFoundException("Book is not reserved");

                    _context.ReservedBooks.Remove(reservedBook);

                    break;
                case (int)BookStatus.BORROWED:
                    BorrowedBook? borrowedBook = await _context.BorrowedBooks
                        .SingleOrDefaultAsync(x => x.BookId == bookId);

                    if (borrowedBook is null) throw new NotFoundException("Book is not borrowed");

                    _context.BorrowedBooks.Remove(borrowedBook);
                    break;
                default:
                    throw new BadRequestException("Book is not reserved or borrowed");
            }

            book.StatusId = (int)BookStatus.AVAILABLE;

            if (await _context.SaveChangesAsync() < 1)
                throw new BadRequestException("Failed to return a book");
        }

        public async Task<PagedList<ReservedBookResponseDto>> GetReservedPagedListAsync(PaginationParams paginationParams)
        {
            IQueryable<ReservedBookResponseDto> reservedBooks = _context.ReservedBooks
                .ProjectTo<ReservedBookResponseDto>(_mapper.ConfigurationProvider)
            .AsQueryable();

            return await PagedList<ReservedBookResponseDto>.CreateAsync(reservedBooks, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<PagedList<BorrowedBookResponseDto>> GetBorrowedPagedListAsync(PaginationParams paginationParams)
        {
            IQueryable<BorrowedBookResponseDto> borrowedBooks = _context.BorrowedBooks
                .ProjectTo<BorrowedBookResponseDto>(_mapper.ConfigurationProvider)
            .AsQueryable();

            return await PagedList<BorrowedBookResponseDto>.CreateAsync(borrowedBooks, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task DeleteAsync(int bookId)
        {
            Book? book = await _context.Books.SingleOrDefaultAsync(x => x.Id == bookId);

            if (book is null) throw new NotFoundException("Book not found");

            _context.Books.Remove(book);

            if (await _context.SaveChangesAsync() < 1)
                throw new BadRequestException("Failed to delete a book");
        }
    }
}
