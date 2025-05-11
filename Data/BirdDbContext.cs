using Microsoft.EntityFrameworkCore;
using BirdLab.Models;

namespace BirdLab.Data
{
    public class BirdDbContext : DbContext
    {
        public DbSet<Bird> Birds { get; set; }

        public BirdDbContext(DbContextOptions<BirdDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bird>()
                .Property(b => b.Species)
                .HasConversion<string>();

            // Seed some initial data
            modelBuilder.Entity<Bird>().HasData(
                new Bird
                {
                    Id = 1,
                    Name = "Red Cardinal",
                    Species = Species.Cardinal,
                    Info = "A beautiful red bird commonly found in North America",
                    CreatedAt = DateTime.UtcNow
                },
                new Bird
                {
                    Id = 2,
                    Name = "Bald Eagle",
                    Species = Species.Eagle,
                    Info = "The national bird of the United States",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
} 