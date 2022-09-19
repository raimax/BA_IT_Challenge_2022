namespace API.Dtos
{
    public class BorrowedBookResponseDto
    {
        public string ReservedBy { get; set; }
        public BookResponseDto Book { get; set; }
    }
}
