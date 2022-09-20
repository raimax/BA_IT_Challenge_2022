using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PublisherRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}