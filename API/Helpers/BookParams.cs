namespace API.Helpers
{
    public class BookParams : PaginationParams
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public string? PublishingDate { get; set; }
        public string? Genre { get; set; }
        public string? Isbn { get; set; }
        public int? Status { get; set; }
    }
}
