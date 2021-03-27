using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using Pokedex.Domain;
using Pokedex.Domain.Queries;
using Xunit;

namespace Pokedex.Tests.Unit
{
    public class BasicPokemonInformationRetrieverTests
    {
        [Fact]
        public async Task CanRetrieveBasicInformationFromTestData()
        {
            // Arrange
            var testDataContents = await File.ReadAllTextAsync("mewtwo.json");
            var testData = JsonSerializer.Deserialize<PokemonSpecies>(
                testDataContents, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });

            var query = new Mock<IPokemonQuery>();
            query.Setup(q => q.Get(It.IsAny<string>()))
                .ReturnsAsync((true, testData));

            var retriever = new BasicPokemonInformationRetriever(query.Object);

            // Act
            var (ok, basicPokemonInfo) = await retriever.Get("mewtwo");

            // Assert
            Assert.True(ok);
            Assert.NotNull(basicPokemonInfo);
            Assert.Equal("mewtwo", basicPokemonInfo.Name);
            Assert.Equal("It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.", basicPokemonInfo.Description);
            Assert.Equal("rare", basicPokemonInfo.Habitat);
            Assert.True(basicPokemonInfo.IsLegendary);
        }

        [Fact]
        public async Task IfNoFlavorTextEntryInEnglishThenNullDescription()
        {
            // Arrange
            var flavorTextEntries = new List<FlavorTextEntry>
            {
                new FlavorTextEntry("Un Pokémon conçu en réorganisant\nles gènes de Mew. On raconte qu’il\ns’agit du Pokémon le plus féroce.", new Resource("fr", "https://pokeapi.co/api/v2/language/5/"))
            };
            var testData = new PokemonSpecies("name", flavorTextEntries, new Resource("rare", "https://pokeapi.co/api/v2/pokemon-habitat/5/"));

            var query = new Mock<IPokemonQuery>();
            query.Setup(q => q.Get(It.IsAny<string>()))
                .ReturnsAsync((true, testData));

            var retriever = new BasicPokemonInformationRetriever(query.Object);

            // Act
            var (ok, basicPokemonInfo) = await retriever.Get("mewtwo");

            // Assert
            Assert.True(ok);
            Assert.NotNull(basicPokemonInfo);
            Assert.Null(basicPokemonInfo.Description);
        }
    }
}
