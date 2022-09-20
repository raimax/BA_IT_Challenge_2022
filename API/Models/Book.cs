namespace API.Models
{
    enum BookStatus
    {
        AVAILABLE = 1,
        RESERVED = 2,
        BORROWED = 3
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public DateTime PublishingDate { get; set; }
        public string Genre { get; set; }
        public string Isbn { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public ReservedBook ReservedBook { get; set; }
        public BorrowedBook BorrowedBook { get; set; }
    }
}
