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
                existingBird.Info = bird.Info;
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
            return _context.Birds.Find(id);
        }

        public override IEnumerable<Bird> GetAll()
        {
            return _context.Birds.ToList();
        }
    }
} 