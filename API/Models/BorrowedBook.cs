namespace API.Models
{
    public class BorrowedBook
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public AppUser User { get; set; }
        public Book Book { get; set; }
    }
}
