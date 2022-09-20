using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public ICollection<AppUserRole> UserRoles { get; set; }
        public ICollection<ReservedBook>? ReservedBooks { get; set; }
        public ICollection<BorrowedBook>? BorrowedBooks { get; set; }
    }
}
