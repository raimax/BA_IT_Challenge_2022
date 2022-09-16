namespace API.Dtos
{
    public class BookResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public AuthorResponseDto Author { get; set; }
        public PublisherResponseDto Publisher { get; set; }
        public DateTime PublishingDate { get; set; }
        public string Genre { get; set; }
        public string Isbn { get; set; }
        public StatusResponseDto Status { get; set; }
    }
}
