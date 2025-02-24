namespace BirdLab.Data
{
    public interface IDataStorage<T>
    {
        void SaveData(List<T> data);
        List<T> LoadData();
    }
}
