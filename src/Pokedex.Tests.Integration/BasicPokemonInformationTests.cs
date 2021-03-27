using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Pokedex.Domain;
using Xunit;

namespace Pokedex.Tests.Integration
{
    public class BasicPokemonInformationTests
    {
        [Theory]
        [InlineData("mewtwo")]
        [InlineData("Mewtwo")]
        [InlineData("MEWTWO")]
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
            Assert.NotNull(content);

            var info = JsonSerializer.Deserialize<BasicPokemonInformation>(
                content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
            Assert.Equal("mewtwo", info.Name);
            Assert.Equal("It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.", info.Description);
            Assert.Equal("rare", info.Habitat);
            Assert.Equal(true, info.IsLegendary);
        }

        [Fact]
        public async Task PokemonDoesNotExistReturnsNotFound()
        {
            // Arrange
            var fixture = new TestFixture();
            var client = fixture.CreateClient();

            // Act
            var response = await client.GetAsync($"/pokemon/idkfhasdkjf");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
