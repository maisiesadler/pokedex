using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokedex.Domain;

namespace Pokedex.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly BasicPokemonInformationRetriever _retriever;
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(
            BasicPokemonInformationRetriever retriever,
            ILogger<PokemonController> logger)
        {
            _retriever = retriever;
            _logger = logger;
        }

        [HttpGet("{pokemonName}")]
        public async Task<IActionResult> Get([FromRoute] string pokemonName)
        {
            var basicPokemonInfo = await _retriever.Get(pokemonName);
            return Ok(basicPokemonInfo.Name);
        }
    }
}
