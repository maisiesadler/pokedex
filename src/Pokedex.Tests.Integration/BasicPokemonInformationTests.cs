using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Tests.Integration
{
    public class BasicPokemonInformationTests
    {
        [Theory]
        [InlineData("mewtwo")]
        public async Task CanGetPokemon(string pokemonName)
        {
            // Arrange
            var fixture = new TestFixture();
            var client = fixture.CreateClient();

            // Act
            var response = await client.GetAsync($"/pokemon/{pokemonName}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(pokemonName, content);
        }
    }
}
