using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Pokedex.Domain;
using Xunit;

namespace Pokedex.Tests.Integration
{
    public class TranslatedPokemonInformationTests
    {
        [Theory]
        [InlineData("mewtwo")]
        [InlineData("Mewtwo")]
        [InlineData("MEWTWO")]
        public async Task LegendaryPokemonTranslatedToYoda(string pokemonName)
        {
            // Arrange
            var fixture = new TestFixture();
            var client = fixture.CreateClient();

            // Act
            var response = await client.GetAsync($"/pokemon/translated/{pokemonName}");

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
            Assert.Equal("Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was.", info.Description);
            Assert.Equal("rare", info.Habitat);
            Assert.True(info.IsLegendary);
        }

        [Theory]
        [InlineData("gengar")]
        [InlineData("Gengar")]
        [InlineData("GENGAR")]
        public async Task NonLegendaryPokemonInCaveTranslatedToYoda(string pokemonName)
        {
            // Arrange
            var fixture = new TestFixture();
            var client = fixture.CreateClient();

            // Act
            var response = await client.GetAsync($"/pokemon/translated/{pokemonName}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);

            var info = JsonSerializer.Deserialize<BasicPokemonInformation>(
                content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
            Assert.Equal("gengar", info.Name);
            Assert.Equal("Under a full moon,This pokémon likes to mimic the shadows of people and laugh at their fright.", info.Description);
            Assert.Equal("cave", info.Habitat);
            Assert.False(info.IsLegendary);
        }

        [Theory]
        [InlineData("greninja")]
        [InlineData("Greninja")]
        [InlineData("GRENINJA")]
        public async Task NonLegendaryPokemonNotInCaveTranslatedToShakespeare(string pokemonName)
        {
            // Arrange
            var fixture = new TestFixture();
            var client = fixture.CreateClient();

            // Act
            var response = await client.GetAsync($"/pokemon/translated/{pokemonName}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);

            var info = JsonSerializer.Deserialize<BasicPokemonInformation>(
                content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
            Assert.Equal("greninja", info.Name);
            Assert.Equal("'t creates throwing stars out of compressed water. At which hour 't spins those folk and throws those folk at high speed,  these stars can did split metal in two.", info.Description);
            Assert.Equal("", info.Habitat);
            Assert.False(info.IsLegendary);
        }

        [Fact]
        public async Task PokemonDoesNotExistReturnsNotFound()
        {
            // Arrange
            var fixture = new TestFixture();
            var client = fixture.CreateClient();

            // Act
            var response = await client.GetAsync($"/pokemon/translated/idkfhasdkjf");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
