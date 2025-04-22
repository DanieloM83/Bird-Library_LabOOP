using BirdLab.Data;
using BirdLab.Models;

namespace BirdLab.Repositories
{
    public class BirdRepository : AbstractRepository<BirdDTO>
    {
        private readonly List<BirdDTO> _birds;

        public BirdRepository(IDataStorage<BirdDTO> storage) : base(storage)
        {
            _birds = dataStorage.LoadData();
        }

        public override void Add(BirdDTO bird)
        {
            _birds.Add(bird);
            dataStorage.SaveData(_birds);
        }

        public override bool Update(BirdDTO bird)
        {
            var existingBird = _birds.FirstOrDefault(b => b.Id == bird.Id);
            if (existingBird != null)
            {
                existingBird.Name = bird.Name;
                existingBird.Species = bird.Species;
                existingBird.Info = bird.Info;

                dataStorage.SaveData(_birds);
                return true;
            }
            return false;
        }

        public override bool Delete(int id)
        {
            var bird = _birds.FirstOrDefault(b => b.Id == id);
            if (bird != null)
            {
                _birds.Remove(bird);
                dataStorage.SaveData(_birds);
                return true;
            }
            return false;
        }

        public override BirdDTO? GetById(int id)
        {
            return _birds.FirstOrDefault(b => b.Id == id);
        }

        public override IEnumerable<BirdDTO> GetAll()
        {
            return _birds;
        }
    }
}
