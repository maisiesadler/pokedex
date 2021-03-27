using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Pokedex.Domain.Queries;
using Pokedex.Queries;
using Xunit;

namespace Pokedex.Tests.Unit
{
    public class PokemonQueryTests
    {
        [Fact]
        public async Task RetrievedFromCacheIfAvailable()
        {
            // Arrange
            var cachedSpecies = new PokemonSpecies("", new List<FlavorTextEntry>(), new Resource("", ""));
            var cache = new Mock<ICache<PokemonSpecies>>();
            cache.Setup(c => c.TryGet(It.IsAny<string>()))
                .ReturnsAsync((true, cachedSpecies));
            var client = new Mock<IPokeApiClient>();

            var query = new PokemonQuery(cache.Object, client.Object);

            // Act
            var result = await query.Get("mewtwo");

            // Assert
            Assert.Equal(cachedSpecies, result);
            cache.Verify(c => c.TryGet("mewtwo"), Times.Once);
            client.Verify(c => c.GetSpecies(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RetrieveNewValueIfNotInCache()
        {
            // Arrange
            var cache = new Mock<ICache<PokemonSpecies>>();
            cache.Setup(c => c.TryGet(It.IsAny<string>()))
                .ReturnsAsync((false, null));
            var species = new PokemonSpecies("", new List<FlavorTextEntry>(), new Resource("", ""));
            var client = new Mock<IPokeApiClient>();
            client.Setup(c => c.GetSpecies(It.IsAny<string>()))
                .ReturnsAsync(species);

            var query = new PokemonQuery(cache.Object, client.Object);

            // Act
            var result = await query.Get("mewtwo");

            // Assert
            Assert.Equal(species, result);
            cache.Verify(c => c.TryGet(It.IsAny<string>()), Times.Once);
            client.Verify(c => c.GetSpecies("mewtwo"), Times.Once);
        }

        [Fact]
        public async Task ValueSetInCacheOnceRetrieved()
        {
            // Arrange
            var cache = new Mock<ICache<PokemonSpecies>>();
            cache.Setup(c => c.TryGet(It.IsAny<string>()))
                .ReturnsAsync((false, null));
            var species = new PokemonSpecies("", new List<FlavorTextEntry>(), new Resource("", ""));
            var client = new Mock<IPokeApiClient>();
            client.Setup(c => c.GetSpecies(It.IsAny<string>()))
                .ReturnsAsync(species);

            var query = new PokemonQuery(cache.Object, client.Object);

            // Act
            var result = await query.Get("mewtwo");

            // Assert
            Assert.Equal(species, result);
            cache.Verify(c => c.Set("mewtwo", species), Times.Once);
        }
    }
}
