using Newtonsoft.Json;

namespace BirdLab.Data
{
    public class JsonDataStorage<T> : IDataStorage<T> where T : class
    {
        private readonly string _filePath;

        public JsonDataStorage(string filePath)
        {
            _filePath = filePath;
        }

        public void SaveData(List<T> data)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

                File.WriteAllText(_filePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while saving data: " + ex.Message);
            }
        }

        public List<T> LoadData()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<T>();
                }
                var jsonData = File.ReadAllText(_filePath);
                var data = JsonConvert.DeserializeObject<List<T>>(jsonData);

                return data ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return new List<T>(); 
            }
        }
    }
}
