using MagicVilla_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option)
            : base(option)
        {
        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>()
                .HasData(new Villa()
                {
                    Id = 1,
                    Name = "Name",
                    Details = "Details",
                    ImageUrl = "",
                    Rate = 50,
                    Occupancy = 10,
                    SqrMt = 80,
                    Amenity = "",
                    CreatedAt = DateTime.Now,
                });
        }
    }
}
