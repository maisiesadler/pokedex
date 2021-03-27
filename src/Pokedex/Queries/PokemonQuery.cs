using System.Threading.Tasks;
using Pokedex.Domain.Queries;

namespace Pokedex.Queries
{
    public class PokemonQuery : IPokemonQuery
    {
        private readonly ICache<PokemonSpecies> _cache;
        private readonly IPokeApiClient _pokeApiClient;

        public PokemonQuery(ICache<PokemonSpecies> cache, IPokeApiClient pokeApiClient)
        {
            _cache = cache;
            _pokeApiClient = pokeApiClient;
        }

        public async Task<PokemonSpecies> Get(string pokemonName)
        {
            var (ok, cached) = await _cache.TryGet(pokemonName);
            if (ok) return cached;

            var species = await _pokeApiClient.GetSpecies(pokemonName);
            await _cache.Set(pokemonName, species);

            return species;
        }
    }
}
