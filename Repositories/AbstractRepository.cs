using BirdLab.Data;

namespace BirdLab.Repositories
{
    
    public abstract class AbstractRepository<T>(IDataStorage<T> storage) where T : class
    {
        protected readonly IDataStorage<T> dataStorage = storage;

        public abstract void Add(T entity); 
        public abstract bool Update(T entity);
        public abstract bool Delete(int id);
        public abstract T GetById(int id); 
        public abstract IEnumerable<T> GetAll();
    }
}
