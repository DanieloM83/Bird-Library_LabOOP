                using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using BirdLab.Data;
using BirdLab.Models;

namespace BirdLab.Repositories
{
    public class EfBirdRepository : AbstractRepository<Bird>
    {
        private readonly BirdDbContext _context;
        
        public EfBirdRepository(BirdDbContext context) : base(null)
        {
            _context = context;
        }
        
        public override void Add(Bird bird)
        {
            _context.Birds.Add(bird);
            _context.SaveChanges();
        }
        
        public override bool Update(Bird bird)
        {
            var existingBird = _context.Birds.Find(bird.Id);
            if (existingBird != null)
            {
                existingBird.Name = bird.Name;
                existingBird.Species = bird.Species;
                existingBird.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        
        public override bool Delete(int id)
        {
            var bird = _context.Birds.Find(id);
            if (bird != null)
            {
                _context.Birds.Remove(bird);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        
        public override Bird? GetById(int id)
        {
            return _context.Birds
                .TagWith("Get bird by ID with details")
                .AsNoTracking()
                .Include(b => b.BirdDetails)
                .FirstOrDefault(b => b.Id == id);
        }
        
        public override IEnumerable<Bird> GetAll()
        {
            return _context.Birds
                .TagWith("Get all birds with details")
                .AsNoTracking()
                .Include(b => b.BirdDetails)
                .ToList();
        }

        // ==================== 9 REQUIRED QUERIES ====================

        // 1. Select bird ID, name, and species. Sort by ID (asc) and name (desc).
        public IEnumerable<object> Query1_GetBirdsSorted()
        {
            return _context.Birds
                .TagWith("Query1: Get birds sorted by ID and name")
                .AsNoTracking()
                .OrderBy(b => b.Id)
                .ThenByDescending(b => b.Name)
                .Select(b => new { b.Id, b.Name, b.Species })
                .ToList();
        }

        // 2. Select birds and locations where location name begins with specified text.
        public IEnumerable<object> Query2_GetBirdsByLocationPrefix(string prefix)
        {
            return _context.Birds
                .TagWith("Query2: Get birds by location prefix")
                .AsNoTracking()
                .Include(b => b.BirdHabitats)
                .ThenInclude(bh => bh.Location)
                .Where(b => b.BirdHabitats.Any(bh => bh.Location.Name.StartsWith(prefix)))
                .Select(b => new { b.Id, b.Name, LocationName = b.BirdHabitats.First().Location.Name })
                .ToList();
        }

        // 3. Select birds observed in September.
        public IEnumerable<object> Query3_GetBirdsObservedInSeptember(int year)
        {
            return _context.Observations
                .TagWith("Query3: Get birds observed in September")
                .AsNoTracking()
                .Include(o => o.Bird)
                .Include(o => o.Location)
                .Where(o => o.ObservationDate.Month == 9 && o.ObservationDate.Year == year)
                .Select(o => new { o.Bird.Name, o.Location, o.ObservationDate })
                .ToList();
        }

        // 4. RAW SQL: Locations with more than X observations.
        public IEnumerable<object> Query4_GetLocationsWithManyObservations(int minCount)
        {
            return _context.Database
                .SqlQuery<LocationObservationCount>($@"
                    /* Query4: Get locations with many observations */
                    SELECT l.Name, COUNT(o.Id) as Count
                    FROM Locations l
                    INNER JOIN Observations o ON l.Id = o.LocationId
                    GROUP BY l.Name
                    HAVING COUNT(o.Id) > {minCount}")
                .AsNoTracking()
                .ToList();
        }

        // 5. Get locations that have specific bird species.
        public IEnumerable<string> Query5_GetLocationsBySpecies(Species species)
        {
            return _context.Locations
                .TagWith("Query5: Get locations by bird species")
                .AsNoTracking()
                .Where(l => l.BirdHabitats.Any(bh => bh.Bird.Species == species))
                .Select(l => l.Name)
                .ToList();
        }

        // 6. RAW SQL: Get average observation count.
        public double Query6_GetAverageObservationCount()
        {
            return _context.Database
                .SqlQuery<double>($@"
                    /* Query6: Get average observation count */
                    SELECT AVG(CAST([Count] AS FLOAT)) as Value FROM Observations")
                .First();
        }

        // 7. Update species for birds with specific name.
        public bool Query7_UpdateBirdSpecies(string birdName, Species newSpecies)
        {
            var birds = _context.Birds
                .TagWith("Query7: Find birds to update species")
                .Where(b => b.Name == birdName)
                .ToList();
                
            foreach (var bird in birds)
            {
                bird.Species = newSpecies;
                bird.UpdatedAt = DateTime.UtcNow;
            }
            _context.SaveChanges();
            return birds.Any();
        }

        // 8. Add two new locations - USING TRANSACTION.
        public void Query8_AddTwoLocations(Location location1, Location location2)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Locations.Add(location1);
                _context.SaveChanges();
                
                _context.Locations.Add(location2);
                _context.SaveChanges();
                
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // 9. RAW SQL: Delete locations without observations in specific year.
        public int Query9_DeleteUnusedLocations(int year)
        {
            return _context.Database.ExecuteSqlRaw($@"
                /* Query9: Delete locations without observations in specific year */
                DELETE FROM Locations 
                WHERE Id NOT IN (
                    SELECT DISTINCT LocationId 
                    FROM Observations 
                    WHERE YEAR(ObservationDate) = {year}
                )");
        }

        // ==================== TRANSACTION EXAMPLE 2 ====================
        
        // Bulk operation with transaction: Add bird with details and habitat
        public bool AddBirdWithDetailsAndHabitat(Bird bird, BirdDetails details, BirdHabitat habitat)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Add bird first
                _context.Birds.Add(bird);
                _context.SaveChanges();
                // Add bird details
                details.BirdId = bird.Id;
                _context.BirdDetails.Add(details);
                _context.SaveChanges();
                // Add habitat relationship
                if (habitat != null)
                {
                    habitat.BirdId = bird.Id;
                    _context.BirdHabitats.Add(habitat);
                    _context.SaveChanges();
                }
                
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {   
                Debug.WriteLine(ex.Message);
                Console.WriteLine(ex.Message);
                transaction.Rollback();
                return false;
            }
        }
    }

    // Helper class for raw SQL query result
    public class LocationObservationCount
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}