using System.Linq;
using System.Threading.Tasks;
using Pokedex.Domain.Queries;

namespace Pokedex.Domain
{
    public class BasicPokemonInformationRetriever
    {
        private readonly IPokemonQuery _pokemonQuery;

        public BasicPokemonInformationRetriever(IPokemonQuery pokemonQuery)
        {
            _pokemonQuery = pokemonQuery;
        }

        public async Task<BasicPokemonInformation> Get(string pokemonName)
        {
            var pokemonSpecies = await _pokemonQuery.Get(pokemonName);

            var description = pokemonSpecies.FlavorTextEntries?.FirstOrDefault(f => f.Language?.Name == "en")?.FlavorText;

            return new BasicPokemonInformation(pokemonSpecies.Name, description, pokemonSpecies.Habitat?.Name, true);
        }
    }
}
