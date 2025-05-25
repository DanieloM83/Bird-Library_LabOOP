using BirdLab.Models;
using BirdLab.Repositories;

namespace BirdLab.Services
{
    public delegate bool BirdFilter(Bird bird);
    public delegate TKey BirdComparator<TKey>(Bird bird);

    public class BirdService
    {
        private readonly EfBirdRepository birdRepository;

        public BirdService(EfBirdRepository repository)
        {
            birdRepository = repository;
        }

        // ==================== BASIC CRUD OPERATIONS ====================

        public List<Bird> GetAllBirds()
        {
            return [.. birdRepository.GetAll()];
        }

        public Bird? GetBirdById(int id)
        {
            return birdRepository.GetById(id);
        }

        public void AddBird(Bird bird)
        {
            birdRepository.Add(bird);
        }

        public bool UpdateBird(Bird bird)
        {
            return birdRepository.Update(bird);
        }

        public bool DeleteBird(int id)
        {
            return birdRepository.Delete(id);
        }

        // ==================== ADVANCED BIRD OPERATIONS ====================

        public bool AddBirdWithDetails(Bird bird, BirdDetails details)
        {
            return birdRepository.AddBirdWithDetailsAndHabitat(bird, details, null);
        }

        public bool AddBirdWithDetailsAndHabitat(Bird bird, BirdDetails details, BirdHabitat? habitat)
        {
            return birdRepository.AddBirdWithDetailsAndHabitat(bird, details, habitat);
        }

        public bool UpdateBirdSpecies(string birdName, Species newSpecies)
        {
            return birdRepository.Query7_UpdateBirdSpecies(birdName, newSpecies);
        }

        // ==================== QUERY METHODS (Using our 9 queries) ====================

        public IEnumerable<object> GetBirdsSortedByIdAndName()
        {
            return birdRepository.Query1_GetBirdsSorted();
        }

        public IEnumerable<object> GetBirdsByLocationPrefix(string locationPrefix)
        {
            return birdRepository.Query2_GetBirdsByLocationPrefix(locationPrefix);
        }

        public IEnumerable<object> GetBirdsObservedInSeptember(int year)
        {
            return birdRepository.Query3_GetBirdsObservedInSeptember(year);
        }

        public IEnumerable<object> GetLocationsWithManyObservations(int minCount)
        {
            return birdRepository.Query4_GetLocationsWithManyObservations(minCount);
        }

        public IEnumerable<string> GetLocationsBySpecies(Species species)
        {
            return birdRepository.Query5_GetLocationsBySpecies(species);
        }

        public double GetAverageObservationCount()
        {
            return birdRepository.Query6_GetAverageObservationCount();
        }

        public void AddTwoLocations(Location location1, Location location2)
        {
            birdRepository.Query8_AddTwoLocations(location1, location2);
        }

        public int DeleteUnusedLocations(int year)
        {
            return birdRepository.Query9_DeleteUnusedLocations(year);
        }

        // ==================== SEARCH AND FILTER OPERATIONS ====================

        public List<Bird> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return GetAllBirds();

            return [.. birdRepository.GetAll()
                .Where(b => b.Name.ToLower().Contains(name.ToLower()))];
        }

        public List<Bird> FilterBySpecies(Species species)
        {
            return [.. birdRepository.GetAll()
                .Where(b => b.Species == species)];
        }

        public List<Bird> FilterByEndangeredStatus(bool isEndangered)
        {
            return [.. birdRepository.GetAll()
                .Where(b => b.BirdDetails != null && b.BirdDetails.IsEndangered == isEndangered)];
        }

        public List<Bird> FilterByWeightRange(double minWeight, double maxWeight)
        {
            return [.. birdRepository.GetAll()
                .Where(b => b.BirdDetails?.AverageWeight >= minWeight && 
                           b.BirdDetails?.AverageWeight <= maxWeight)];
        }

        public List<Bird> FilterByDiet(string diet)
        {
            if (string.IsNullOrWhiteSpace(diet))
                return GetAllBirds();

            return [.. birdRepository.GetAll()
                .Where(b => b.BirdDetails?.Diet?.ToLower().Contains(diet.ToLower()) == true)];
        }

        public List<Bird> GetBirdsByDelegate(BirdFilter filter)
        {
            var birds = birdRepository.GetAll();
            return [.. birds.Where(b => filter(b))];
        }

        // ==================== SORTING OPERATIONS ====================

        public List<Bird> GetSortedBirds<TKey>(BirdComparator<TKey> sortBy)
        {
            return [.. birdRepository.GetAll().OrderBy(b => sortBy(b))];
        }

        public List<Bird> SortByNameDescendingThenSpecies()
        {
            return [.. birdRepository.GetAll()
                .OrderByDescending(b => b.Name)
                .ThenBy(b => b.Species)];
        }

        public List<Bird> SortByWeight()
        {
            return [.. birdRepository.GetAll()
                .OrderBy(b => b.BirdDetails?.AverageWeight ?? 0)];
        }

        public List<Bird> SortByObservationCount()
        {
            return [.. birdRepository.GetAll()
                .OrderByDescending(b => b.Observations?.Count ?? 0)];
        }

        // ==================== STATISTICAL OPERATIONS ====================

        public int GetTotalBirdsCount()
        {
            return birdRepository.GetAll().Count();
        }

        public Species? GetMostCommonSpecies()
        {
            var maxSpeciesGroup = birdRepository.GetAll()
                .GroupBy(b => b.Species)
                .Select(group => new { Species = group.Key, Count = group.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefault();
            
            return maxSpeciesGroup?.Species;
        }

        public double GetAverageWeight()
        {
            var weights = birdRepository.GetAll()
                .Where(b => b.BirdDetails?.AverageWeight.HasValue == true)
                .Select(b => b.BirdDetails!.AverageWeight!.Value);
            
            return weights.Any() ? weights.Average() : 0;
        }

        public int GetEndangeredSpeciesCount()
        {
            return birdRepository.GetAll()
                .Count(b => b.BirdDetails?.IsEndangered == true);
        }

        public Dictionary<Species, int> GetSpeciesDistribution()
        {
            return birdRepository.GetAll()
                .GroupBy(b => b.Species)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public Dictionary<string, int> GetObservationsByLocation()
        {
            return birdRepository.GetAll()
                .SelectMany(b => b.Observations ?? new List<Observation>())
                .GroupBy(o => o.Location?.Name ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // ==================== PAGINATION ====================

        public List<Bird> GetPagedBirds(int pageNumber = 1, int pageSize = 10)
        {
            return [.. birdRepository.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)];
        }

        public (List<Bird> birds, int totalCount, int totalPages) GetPagedBirdsWithInfo(int pageNumber = 1, int pageSize = 10)
        {
            var allBirds = birdRepository.GetAll();
            var totalCount = allBirds.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            
            var pagedBirds = allBirds
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (pagedBirds, totalCount, totalPages);
        }

        // ==================== DATA PROJECTION ====================

        public List<(string Name, Species Species)> GetBasicInfo()
        {
            return [.. birdRepository.GetAll()
                .Select(b => new ValueTuple<string, Species>(b.Name, b.Species))];
        }

        public List<object> GetBirdSummaries()
        {
            return [.. birdRepository.GetAll()
                .Select(b => new 
                {
                    b.Id,
                    b.Name,
                    b.Species,
                    IsEndangered = b.BirdDetails?.IsEndangered ?? false,
                    ObservationCount = b.Observations?.Count ?? 0,
                    Weight = b.BirdDetails?.AverageWeight,
                    HasDetails = b.BirdDetails != null
                })];
        }

        // ==================== SET OPERATIONS ====================

        public static List<Bird> GetBirdsIntersection(List<Bird> birds1, List<Bird> birds2)
        {
            return [.. birds1.IntersectBy(birds2.Select(b => b.Id), b => b.Id)];
        }

        public static List<Bird> GetBirdsSetMinus(List<Bird> birds1, List<Bird> birds2)
        {
            return [.. birds1.ExceptBy(birds2.Select(b => b.Id), b => b.Id)];
        }

        public static List<Bird> GetBirdsUnion(List<Bird> birds1, List<Bird> birds2)
        {
            return [.. birds1.UnionBy(birds2, b => b.Id)];
        }

        // ==================== UTILITY METHODS ====================

        public EfBirdRepository GetRepository()
        {
            return birdRepository;
        }

        public bool BirdExists(int id)
        {
            return birdRepository.GetById(id) != null;
        }

        public bool BirdExistsByName(string name)
        {
            return birdRepository.GetAll().Any(b => 
                string.Equals(b.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        // ==================== VALIDATION METHODS ====================

        public (bool isValid, string errorMessage) ValidateBird(Bird bird)
        {
            if (string.IsNullOrWhiteSpace(bird.Name))
                return (false, "Bird name is required.");

            if (bird.Name.Length > 100)
                return (false, "Bird name cannot exceed 100 characters.");

            if (BirdExistsByName(bird.Name))
                return (false, "A bird with this name already exists.");

            return (true, string.Empty);
        }

        public (bool isValid, string errorMessage) ValidateBirdDetails(BirdDetails details)
        {
            if (string.IsNullOrWhiteSpace(details.Info))
                return (false, "Bird info is required.");

            if (details.Info.Length > 500)
                return (false, "Bird info cannot exceed 500 characters.");

            if (details.AverageLength < 0 || details.AverageLength > 500)
                return (false, "Average length must be between 0 and 500 cm.");

            if (details.AverageWeight < 0 || details.AverageWeight > 50000)
                return (false, "Average weight must be between 0 and 50,000 grams.");

            return (true, string.Empty);
        }
    }
}