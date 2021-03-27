using System.Threading.Tasks;

namespace Pokedex.Domain.Queries
{
    public interface IPokemonQuery
    {
        Task<(bool, PokemonSpecies)> Get(string pokemonName);
    }
}
