namespace API.Dtos
{
    public class BorrowedBookResponseDto
    {
        public string BorrowedBy { get; set; }
        public BookResponseDto Book { get; set; }
    }
}
