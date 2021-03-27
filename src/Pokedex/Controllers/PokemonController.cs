﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            pokemonName = pokemonName?.ToLower();
            var (ok, basicPokemonInfo) = await _retriever.Get(pokemonName);
            if (!ok) return NotFound();

            return Ok(basicPokemonInfo);
        }
    }
}
