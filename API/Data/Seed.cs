using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            try
            {
                await CreateRoles(roleManager);

                await CreateUser(userManager, "admin", new[] { "Admin" });
                await CreateUser(userManager, "regular@gmail.com", new[] { "Regular" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Migration failed" + ex.Message);
                throw;
            }
        }

        private static async Task CreateRoles(RoleManager<AppRole> roleManager)
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

        private static async Task CreateUser(UserManager<AppUser> userManager, string username, string[] roles)
        {
            AppUser user = new()
            {
                UserName = username
            };

            await userManager.CreateAsync(user, "password");
            await userManager.AddToRolesAsync(user, roles);
        }
    }
}
