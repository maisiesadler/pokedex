using System.Threading.Tasks;

namespace Pokedex.Domain.Queries
{
    public interface ITranslationQuery
    {
        Task<string> Translate(Translation translation, string original);
    }
}
