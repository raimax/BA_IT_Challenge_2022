using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class AuthorRequestDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}