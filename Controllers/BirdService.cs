using BirdLab.Models;
using BirdLab.Repositories;

namespace BirdLab.Services
{
    public class BirdService
    {
        private readonly BirdRepository birdRepository;

        public BirdService(BirdRepository repository)
        {
            birdRepository = repository;
        }

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
    }
}
