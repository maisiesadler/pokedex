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

        public async Task<(bool, BasicPokemonInformation)> Get(string pokemonName)
        {
            var (ok, pokemonSpecies) = await _pokemonQuery.Get(pokemonName);
            if (!ok) return (false, null);

            var description = pokemonSpecies.FlavorTextEntries?.FirstOrDefault(f => f.Language?.Name == "en")?.FlavorText ?? string.Empty;

            return (true, new BasicPokemonInformation(pokemonSpecies.Name, description, pokemonSpecies.Habitat?.Name ?? string.Empty, pokemonSpecies.IsLegendary));
        }
    }
}
