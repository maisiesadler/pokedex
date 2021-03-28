using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Pokedex.Domain;
using Pokedex.Domain.Queries;
using Xunit;

namespace Pokedex.Tests.Unit
{
    public class TranslatedPokemonInformationRetrieverTests
    {
        [Fact]
        public async Task LegendaryPokemonDescriptionTranslatedToYoda()
        {
            // Arrange
            var flavorTextEntries = new List<FlavorTextEntry>
            {
                new FlavorTextEntry("desc", new Resource("en", "https://pokeapi.co/api/v2/language/5/"))
            };
            var testData = new PokemonSpecies("name", true, flavorTextEntries, new Resource("rare", "https://pokeapi.co/api/v2/pokemon-habitat/5/"));

            var query = new Mock<IPokemonQuery>();
            query.Setup(q => q.Get(It.IsAny<string>()))
                .ReturnsAsync((true, testData));

            var translator = new Mock<ITranslationQuery>();
            translator.Setup(t => t.Translate(Translation.Yoda, It.IsAny<string>()))
                .ReturnsAsync("yoda-translation");
            query.Setup(q => q.Get(It.IsAny<string>()))
                .ReturnsAsync((true, testData));

            var retriever = new TranslatedPokemonInformationRetriever(query.Object, translator.Object);

            // Act
            var (ok, translatedInfo) = await retriever.Get("mewtwo");

            // Assert
            Assert.True(ok);
            Assert.NotNull(translatedInfo);
            Assert.Equal("name", translatedInfo.Name);
            Assert.Equal("yoda-translation", translatedInfo.Description);

            translator.Verify(t => t.Translate(Translation.Yoda, "desc"), Times.Once);
        }

        [Fact]
        public async Task NonLegendaryPokemonInCaveDescriptionTranslatedToYoda()
        {
            // Arrange
            var flavorTextEntries = new List<FlavorTextEntry>
            {
                new FlavorTextEntry("desc", new Resource("en", "https://pokeapi.co/api/v2/language/5/"))
            };
            var testData = new PokemonSpecies("name", false, flavorTextEntries, new Resource("cave", "https://pokeapi.co/api/v2/pokemon-habitat/5/"));

            var query = new Mock<IPokemonQuery>();
            query.Setup(q => q.Get(It.IsAny<string>()))
                .ReturnsAsync((true, testData));

            var translator = new Mock<ITranslationQuery>();
            translator.Setup(t => t.Translate(Translation.Yoda, It.IsAny<string>()))
                .ReturnsAsync("yoda-translation");
            query.Setup(q => q.Get(It.IsAny<string>()))
                .ReturnsAsync((true, testData));

            var retriever = new TranslatedPokemonInformationRetriever(query.Object, translator.Object);

            // Act
            var (ok, translatedInfo) = await retriever.Get("mewtwo");

            // Assert
            Assert.True(ok);
            Assert.NotNull(translatedInfo);
            Assert.Equal("name", translatedInfo.Name);
            Assert.Equal("yoda-translation", translatedInfo.Description);

            translator.Verify(t => t.Translate(Translation.Yoda, "desc"), Times.Once);
        }

        [Fact]
        public async Task NonLegendaryPokemonNotInCaveDescriptionTranslatedToShakespeare()
        {
            // Arrange
            var flavorTextEntries = new List<FlavorTextEntry>
            {
                new FlavorTextEntry("desc", new Resource("en", "https://pokeapi.co/api/v2/language/5/"))
            };
            var testData = new PokemonSpecies("name", false, flavorTextEntries, new Resource("rare", "https://pokeapi.co/api/v2/pokemon-habitat/5/"));

            var query = new Mock<IPokemonQuery>();
            query.Setup(q => q.Get(It.IsAny<string>()))
                .ReturnsAsync((true, testData));

            var translator = new Mock<ITranslationQuery>();
            translator.Setup(t => t.Translate(Translation.Shakespeare, It.IsAny<string>()))
                .ReturnsAsync("shakespeare-translation");
            query.Setup(q => q.Get(It.IsAny<string>()))
                .ReturnsAsync((true, testData));

            var retriever = new TranslatedPokemonInformationRetriever(query.Object, translator.Object);

            // Act
            var (ok, translatedInfo) = await retriever.Get("mewtwo");

            // Assert
            Assert.True(ok);
            Assert.NotNull(translatedInfo);
            Assert.Equal("name", translatedInfo.Name);
            Assert.Equal("shakespeare-translation", translatedInfo.Description);

            translator.Verify(t => t.Translate(Translation.Shakespeare, "desc"), Times.Once);
        }
    }
}
