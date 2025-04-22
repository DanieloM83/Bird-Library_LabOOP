using BirdLab.Models;
using BirdLab.Repositories;

namespace BirdLab.Services
{
    public delegate bool BirdFilter(BirdDTO bird);
    public delegate TKey BirdComparator<TKey>(BirdDTO bird);


    public class BirdService(BirdRepository repository)
    {
        private readonly BirdRepository birdRepository = repository;

        public List<BirdDTO> GetAllBirds()
        {
            return [.. birdRepository.GetAll()];
        }

        public void AddBird(BirdDTO bird)
        {
            birdRepository.Add(bird);
        }

        public bool DeleteBird(int id)
        {
            return birdRepository.Delete(id);
        }

        public List<BirdDTO> GetSortedBirds<TKey>(BirdComparator<TKey> sortBy)
        {
            return [.. birdRepository.GetAll().OrderBy((b) => sortBy(b))];
        }

        public List<BirdDTO> GetBirdsByDelegate(BirdFilter filter)
        {
            var birds = birdRepository.GetAll();
            return [.. birds.Where((b) => filter(b))];
        }

        public List<BirdDTO> SearchByName(string name)
        {
            return [.. birdRepository.GetAll().Where(b => b.Name.ToLower().Contains(name.ToLower()))];
        }

        public List<BirdDTO> FilterBySpecies(Species species)
        {
            return [.. birdRepository.GetAll().Where(b => b.Species == species)];
        }

        public List<BirdDTO> SortByNameDescendingThenSpecies()
        {
            return [.. birdRepository.GetAll()
                .OrderByDescending(b => b.Name)
                .ThenBy(b => b.Species)];
        }

        public List<(string Name, Species Species)> GetBasicInfo() {
            return [.. birdRepository.GetAll().Select<BirdDTO, (string Name, Species Species)>(b => new (b.Name, b.Species))];
        }

        public List<BirdDTO> GetPagedBirds(int pageNumber = 1, int pageSize = 10) {
            return [.. birdRepository.GetAll().Skip((pageNumber - 1) * pageSize).Take(pageSize)];
        }

        public int GetTotalBirdsCount()
        {
            return birdRepository.GetAll().Count();
        }

        public Species? GetMaxSpecies()
        {
            var maxSpeciesGroup = birdRepository.GetAll().GroupBy(static b => b.Species).Select(static group => new
            {
                Species = group.Key,
                Count = group.Count()
            }).OrderByDescending(static g => g.Count).FirstOrDefault();
            return maxSpeciesGroup?.Species;
        }

        public static List<BirdDTO> GetBirdsIntersection(List<BirdDTO> birds1, List<BirdDTO> birds2)
        {
            return [.. birds1.IntersectBy(birds2.Select(b => b.Id), b => b.Id)];
        }

        public static List<BirdDTO> GetBirdsSetMinus(List<BirdDTO> birds1, List<BirdDTO> birds2)
        {
            return [.. birds1.ExceptBy(birds2.Select(b => b.Id), b => b.Id)];
        }
    }
}
