using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<FileCache<T>> _logger;

        public FileCache(string location, ILogger<FileCache<T>> logger)
        {
            _location = location;
            _logger = logger;

            if (!Directory.Exists(_location))
                Directory.CreateDirectory(_location);
        }

        public async Task Set(string key, T value)
        {
            try
            {
                var path = GetLocation(key);
                var contents = JsonSerializer.Serialize(value);
                await File.WriteAllTextAsync(path, contents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not cache value for {CacheKey}.", key);
            }
        }

        public async Task<(bool, T)> TryGet(string key)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not get cached value for {CacheKey}.", key);
                return (false, default);
            }
        }

        private string GetLocation(string key) => $"{_location}/{key}.json";
    }
}
