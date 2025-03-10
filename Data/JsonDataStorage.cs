using Newtonsoft.Json;

namespace BirdLab.Data
{
    public interface IJsonDataStorage<T> : IDataStorage<T> 
    {
        string _filePath { get; }
    }


    public class JsonDataStorage<T>(string filePath = "birds.json") : IJsonDataStorage<T> where T : class
    {
        public string _filePath { get; } = filePath;

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
                    return [];
                }
                var jsonData = File.ReadAllText(_filePath);
                var data = JsonConvert.DeserializeObject<List<T>>(jsonData);

                return data ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return []; 
            }
        }
    }
}
