namespace API.Dtos
{
    public class ReservedBookResponseDto
    {
        public string ReservedBy { get; set; }
        public BookResponseDto Book { get; set; }
    }
}
