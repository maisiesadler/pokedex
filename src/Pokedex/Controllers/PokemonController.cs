using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokedex.Domain;

namespace Pokedex.Controllers
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly BasicPokemonInformationRetriever _basicRetriever;
        private readonly TranslatedPokemonInformationRetriever _translatedRetriever;
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(
            BasicPokemonInformationRetriever basicRetriever,
            TranslatedPokemonInformationRetriever translatedRetriever,
            ILogger<PokemonController> logger)
        {
            _basicRetriever = basicRetriever;
            _translatedRetriever = translatedRetriever;
            _logger = logger;
        }

        [HttpGet("{pokemonName}")]
        public async Task<IActionResult> Get([FromRoute] string pokemonName)
        {
            pokemonName = pokemonName?.ToLower();
            var (ok, basicPokemonInfo) = await _basicRetriever.Get(pokemonName);
            if (!ok) return NotFound();

            return Ok(basicPokemonInfo);
        }

        [HttpGet("translated/{pokemonName}")]
        public async Task<IActionResult> TranslatedGet([FromRoute] string pokemonName)
        {
            pokemonName = pokemonName?.ToLower();
            var (ok, basicPokemonInfo) = await _translatedRetriever.Get(pokemonName);
            if (!ok) return NotFound();

            return Ok(basicPokemonInfo);
        }
    }
}
