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
    }
}
