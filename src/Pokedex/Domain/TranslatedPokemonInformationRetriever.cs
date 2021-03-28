using System.Threading.Tasks;
using Pokedex.Domain.Queries;

namespace Pokedex.Domain
{
    public class TranslatedPokemonInformationRetriever
    {
        private readonly BasicPokemonInformationRetriever _basicInfoRetriever;
        private readonly ITranslationQuery _translationQuery;

        public TranslatedPokemonInformationRetriever(IPokemonQuery pokemonQuery, ITranslationQuery translationQuery)
        {
            _basicInfoRetriever = new BasicPokemonInformationRetriever(pokemonQuery);
            _translationQuery = translationQuery;
        }

        public async Task<(bool, TranslatedPokemonInformation)> Get(string pokemonName)
        {
            var (ok, basicPokemonInfo) = await _basicInfoRetriever.Get(pokemonName);
            if (!ok) return (false, null);

            var translation = GetTranslation(basicPokemonInfo);
            var translated = await _translationQuery.Translate(translation, basicPokemonInfo.Description);

            return (true, new TranslatedPokemonInformation(basicPokemonInfo, translated));
        }

        private static Translation GetTranslation(BasicPokemonInformation basicPokemonInformation)
        {
            if (basicPokemonInformation.Habitat == "cave" || basicPokemonInformation.IsLegendary)
                return Translation.Yoda;

            return Translation.Shakespeare;
        }
    }
}
