using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokedex.Queries
{
    public interface ICache<T>
    {
        Task<(bool, T)> TryGet(string key);
        Task Set(string key, T value);
    }

    public class FileCache<T> : ICache<T>
    {
        private readonly string _location;

        public FileCache(string location)
        {
            _location = location;

            if (!Directory.Exists(_location))
                Directory.CreateDirectory(_location);
        }

        public async Task Set(string key, T value)
        {
            var path = GetLocation(key);
            var contents = JsonSerializer.Serialize(value);
            await File.WriteAllTextAsync(path, contents);
        }

        public async Task<(bool, T)> TryGet(string key)
        {
            var path = GetLocation(key);
            if (File.Exists(path))
            {
                var contents = await File.ReadAllTextAsync(path);
                var value = JsonSerializer.Deserialize<T>(contents);
                return (true, value);
            }

            return (false, default);
        }

        private string GetLocation(string key) => $"{_location}/{key}.json";
    }
}
