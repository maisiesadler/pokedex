using System.Threading.Tasks;

namespace Pokedex.Domain.Queries
{
    public interface IPokemonQuery
    {
        Task<PokemonSpecies> Get(string pokemonName);
    }
}
