using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wunderwaffe_Shop.Models;

namespace Wunderwaffe_Shop.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        // Usuń logikę wykonywania SQL w OnConfiguring
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     base.OnConfiguring(optionsBuilder);
        //     // ExecuteSqlScript("wwwroot/SeedData.sql");
        // }

        // Możesz zachować metodę ExecuteSqlScript, ale przenieść jej użycie do Program.cs
        public void ExecuteSqlScript(string filePath)
        {
            if (File.Exists(filePath))
            {
                string sql = File.ReadAllText(filePath);
                Database.ExecuteSqlRaw(sql);
            }
            else
            {
                throw new FileNotFoundException($"Plik SQL nie został znaleziony: {filePath}");
            }
        }
    }
}
