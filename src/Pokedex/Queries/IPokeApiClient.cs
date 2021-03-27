using System.Threading.Tasks;
using Pokedex.Domain.Queries;
using Refit;

namespace Pokedex.Queries
{
    public interface IPokeApiClient
    {
        [Get("/pokemon-species/{pokemonName}")]
        Task<PokemonSpecies> GetSpecies(string pokemonName);
    }
}
