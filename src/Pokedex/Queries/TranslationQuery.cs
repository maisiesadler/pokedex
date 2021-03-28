using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pokedex.Domain.Queries;
using Pokedex.Queries.Models;

namespace Pokedex.Queries
{
    public class TranslationQuery : ITranslationQuery
    {
        private readonly static Regex _whitepaceRegex = new Regex("\\s+", RegexOptions.None);
        private readonly ICache<string> _cache;
        private readonly IFunTranslationsApiClient _funTranslationsApiClient;
        private readonly ILogger<TranslationQuery> _logger;

        public TranslationQuery(
            ICache<string> cache,
            IFunTranslationsApiClient funTranslationsApiClient,
            ILogger<TranslationQuery> logger)
        {
            _cache = cache;
            _funTranslationsApiClient = funTranslationsApiClient;
            _logger = logger;
        }

        public async Task<string> Translate(Translation translation, string original)
        {
            try
            {
                var cacheKey = CacheKey(translation, original);
                var (ok, cached) = await _cache.TryGet(cacheKey);
                if (ok) return cached;

                var response = translation switch
                {
                    Translation.Yoda => await _funTranslationsApiClient.YodaTranslation(original),
                    _ => await _funTranslationsApiClient.ShakespeareTranslation(original),
                };

                var translated = response?.Contents?.Translated;

                await _cache.Set(cacheKey, translated);

                return translated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not translate description '{Original}' to {Translation}", original, translation);
                return original;
            }
        }

        private string CacheKey(Translation translation, string original) => $"{translation}-{_whitepaceRegex.Replace(original, " ")}";
    }
}
