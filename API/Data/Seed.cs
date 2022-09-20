using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        private readonly DatabaseContext _dbContext;

        public Seed(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            try
            {
                await CreateRoles(roleManager);

                await CreateUser(userManager, "admin", new[] { "Admin" });
                await CreateUser(userManager, "user", new[] { "Regular" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Migration failed" + ex.Message);
                throw;
            }
        }

        private async Task CreateRoles(RoleManager<AppRole> roleManager)
        {
            List<AppRole> roles = new List<AppRole>
                {
                    new AppRole {Name = "Regular"},
                    new AppRole {Name = "Admin"},
                };

            foreach (AppRole role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }

        private async Task CreateUser(UserManager<AppUser> userManager, string username, string[] roles)
        {
            AppUser user = new()
            {
                UserName = username
            };

            await userManager.CreateAsync(user, "password");
            await userManager.AddToRolesAsync(user, roles);
        }

        public async Task SeedBookStatus()
        {
            if (await _dbContext.Status.AnyAsync()) return;

            try
            {
                await CreateStatus("available");
                await CreateStatus("reserved");
                await CreateStatus("borrowed");
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Migration failed" + ex.Message);
                throw;
            }
        }

        private async Task CreateStatus(string statusName)
        {
            Status status = new Status
            {
                Name = statusName
            };

            await _dbContext.Status.AddAsync(status);
        }

        public async Task SeedBooks()
        {
            if (await _dbContext.Books.AnyAsync()) return;

            var bookData = await File.ReadAllTextAsync("Data/books.json");
            var books = JsonSerializer.Deserialize<List<Book>>(bookData);

            if (books is null) return;

            Status? status = await _dbContext.Status.FindAsync(1);
            if (status is not null)
            {
                foreach (var book in books)
                {
                    Author? author = await _dbContext.Authors
                        .SingleOrDefaultAsync(x =>
                            x.FirstName.ToLower() == book.Author.FirstName.ToLower() &&
                            x.LastName.ToLower() == book.Author.LastName.ToLower());

                    Publisher? publisher = await _dbContext.Publishers
                        .SingleOrDefaultAsync(x => x.Name.ToLower() == book.Publisher.Name.ToLower());

                    if (author is not null) book.Author = author;
                    if (publisher is not null) book.Publisher = publisher;

                    book.Status = status;
                }
            }

            await _dbContext.Books.AddRangeAsync(books);
            await _dbContext.SaveChangesAsync();

        }
    }
}
