using Microsoft.AspNetCore.Identity;
using Wunderwaffe_Shop.Models;

namespace Wunderwaffe_Shop.Services
{
    public class DatabaseInitializer
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser>? userManager,
            RoleManager<IdentityRole>? roleManager)
        {
            if (userManager == null || roleManager == null)
            {
                Console.WriteLine("userManager lub roleManager jest null => exit");
                return;
            }

            var exists = await roleManager.RoleExistsAsync("admin");
            if (!exists)
            {
                Console.WriteLine("Administrator nie został stworzony lub zdefiniowany");
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            exists = await roleManager.RoleExistsAsync("sprzedawca");
            if (!exists)
            {
                Console.WriteLine("Sprzedawca nie został stworzony lub zdefiniowany");
                await roleManager.CreateAsync(new IdentityRole("sprzedawca"));
            }

            exists = await roleManager.RoleExistsAsync("klient");
            if (!exists)
            {
                Console.WriteLine("Klient nie został stworzony lub zdefiniowany");
                await roleManager.CreateAsync(new IdentityRole("klient"));
            }



            var adminUsers = await userManager.GetUsersInRoleAsync("admin");
            if (adminUsers.Any())
            {
                Console.WriteLine("Admin już istnienie => exit");
                return;
            }

            var user = new ApplicationUser()
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "admin@admin.com", 
                Email = "admin@admin.com",
                CreatedAt = DateTime.Now,
            };

            string initialPassword = "admin123";


            var result = await userManager.CreateAsync(user, initialPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "admin");
                Console.WriteLine("Admin user created successfully! Please update the initial password!");
                Console.WriteLine("Email: " + user.Email);
                Console.WriteLine("Initial password: " + initialPassword);
            }
        }
    }
}
