using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class BookRequestDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int PublisherId { get; set; }
        [Required]
        public DateTime PublishingDate { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string Isbn { get; set; }
    }
}
