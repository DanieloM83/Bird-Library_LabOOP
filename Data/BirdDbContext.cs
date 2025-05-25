using Microsoft.EntityFrameworkCore;
using BirdLab.Models;

namespace BirdLab.Data
{
    public class BirdDbContext : DbContext
    {
        public BirdDbContext(DbContextOptions<BirdDbContext> options) : base(options)
        {
        }
        
        public DbSet<Bird> Birds { get; set; }
        public DbSet<BirdDetails> BirdDetails { get; set; }
        public DbSet<Observation> Observations { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<BirdHabitat> BirdHabitats { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Bird entity
            modelBuilder.Entity<Bird>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Name).IsRequired().HasMaxLength(100);
                entity.Property(b => b.Species).IsRequired();
                entity.Property(b => b.CreatedAt).IsRequired();
                
                // Configure one-to-one relationship with BirdDetails
                entity.HasOne(b => b.BirdDetails)
                      .WithOne(bd => bd.Bird)
                      .HasForeignKey<BirdDetails>(bd => bd.BirdId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configure BirdDetails entity
            modelBuilder.Entity<BirdDetails>(entity =>
            {
                entity.HasKey(bd => bd.BirdId);
                entity.Property(bd => bd.Info).IsRequired().HasMaxLength(500);
                entity.Property(bd => bd.Description).HasMaxLength(1000);
                entity.Property(bd => bd.Diet).HasMaxLength(200);
                entity.Property(bd => bd.Behavior).HasMaxLength(200);
            });
            
            // Configure Observation entity
            modelBuilder.Entity<Observation>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.ObservationDate).IsRequired();
                entity.Property(o => o.Count).IsRequired();
                entity.Property(o => o.Notes).HasMaxLength(500);
                entity.Property(o => o.ObserverName).HasMaxLength(100);
                
                // Configure relationships
                entity.HasOne(o => o.Bird)
                      .WithMany(b => b.Observations)
                      .HasForeignKey(o => o.BirdId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(o => o.Location)
                      .WithMany(l => l.Observations)
                      .HasForeignKey(o => o.LocationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            
            // Configure Location entity
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Name).IsRequired().HasMaxLength(200);
                entity.Property(l => l.Latitude).IsRequired();
                entity.Property(l => l.Longitude).IsRequired();
                entity.Property(l => l.Country).HasMaxLength(100);
                entity.Property(l => l.Region).HasMaxLength(100);
                entity.Property(l => l.Description).HasMaxLength(500);
                
                // Create unique index for coordinates
                entity.HasIndex(l => new { l.Latitude, l.Longitude })
                      .HasDatabaseName("IX_Location_Coordinates");
            });
            
            // Configure BirdHabitat entity (many-to-many)
            modelBuilder.Entity<BirdHabitat>(entity =>
            {
                entity.HasKey(bh => bh.Id);
                entity.Property(bh => bh.Season).HasMaxLength(50);
                
                // Configure relationships
                entity.HasOne(bh => bh.Bird)
                      .WithMany(b => b.BirdHabitats)
                      .HasForeignKey(bh => bh.BirdId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(bh => bh.Location)
                      .WithMany(l => l.BirdHabitats)
                      .HasForeignKey(bh => bh.LocationId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Create unique index to prevent duplicate bird-location combinations
                entity.HasIndex(bh => new { bh.BirdId, bh.LocationId, bh.Season })
                      .IsUnique()
                      .HasDatabaseName("IX_BirdHabitat_Unique");
            });
            
            // Seed data (optional)
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed some initial data
            modelBuilder.Entity<Location>().HasData(
                new Location { Id = 1, Name = "Central Park", Latitude = 40.7829, Longitude = -73.9654, Country = "USA", Region = "New York" },
                new Location { Id = 2, Name = "Yellowstone National Park", Latitude = 44.4280, Longitude = -110.5885, Country = "USA", Region = "Wyoming" }
            );
        }
    }
}