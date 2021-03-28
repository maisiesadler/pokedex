using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Pokedex.Domain.Queries;
using Pokedex.Queries;
using Pokedex.Queries.Models;
using Xunit;

namespace Pokedex.Tests.Unit
{
    public class TranslationQueryTests
    {
        [Theory]
        [InlineData(Translation.Shakespeare)]
        [InlineData(Translation.Yoda)]
        public async Task ValueRetrievedFromCacheIfAvailable(Translation translation)
        {
            // Arrange
            var cache = new Mock<ICache<string>>();
            cache.Setup(c => c.TryGet(It.IsAny<string>()))
                .ReturnsAsync((true, "cached-description"));
            var client = new Mock<IFunTranslationsApiClient>();

            var query = new TranslationQuery(cache.Object, client.Object, NullLogger<TranslationQuery>.Instance);

            // Act
            var translated = await query.Translate(translation, "some-description");

            // Assert
            Assert.Equal("cached-description", translated);
            cache.Verify(c => c.TryGet($"{translation}-some-description"), Times.Once);
            client.Verify(c => c.YodaTranslation(It.IsAny<string>()), Times.Never);
            client.Verify(c => c.ShakespeareTranslation(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task YodaTranslationValueSetInCacheOnceRetrieved()
        {
            // Arrange
            var cache = new Mock<ICache<string>>();
            cache.Setup(c => c.TryGet(It.IsAny<string>()))
                .ReturnsAsync((false, null));
            var client = new Mock<IFunTranslationsApiClient>();
            client.Setup(c => c.YodaTranslation(It.IsAny<string>()))
                .ReturnsAsync(new FunTranslationResponse(new FunTranslationResponseContents("translated")));

            var query = new TranslationQuery(cache.Object, client.Object, NullLogger<TranslationQuery>.Instance);

            // Act
            var translated = await query.Translate(Translation.Yoda, "some-description");

            // Assert
            Assert.Equal("translated", translated);
            cache.Verify(c => c.Set("Yoda-some-description", translated), Times.Once);
        }

        [Fact]
        public async Task ShakespeareTranslationValueSetInCacheOnceRetrieved()
        {
            // Arrange
            var cache = new Mock<ICache<string>>();
            cache.Setup(c => c.TryGet(It.IsAny<string>()))
                .ReturnsAsync((false, null));
            var client = new Mock<IFunTranslationsApiClient>();
            client.Setup(c => c.ShakespeareTranslation(It.IsAny<string>()))
                .ReturnsAsync(new FunTranslationResponse(new FunTranslationResponseContents("translated")));

            var query = new TranslationQuery(cache.Object, client.Object, NullLogger<TranslationQuery>.Instance);

            // Act
            var translated = await query.Translate(Translation.Shakespeare, "some-description");

            // Assert
            Assert.Equal("translated", translated);
            cache.Verify(c => c.Set("Shakespeare-some-description", translated), Times.Once);
        }

        [Fact]
        public async Task CacheKeyIsNormalised()
        {
            // Arrange
            var cache = new Mock<ICache<string>>();
            cache.Setup(c => c.TryGet(It.IsAny<string>()))
                .ReturnsAsync((false, null));
            var client = new Mock<IFunTranslationsApiClient>();
            client.Setup(c => c.ShakespeareTranslation(It.IsAny<string>()))
                .ReturnsAsync(new FunTranslationResponse(new FunTranslationResponseContents("translated")));

            var query = new TranslationQuery(cache.Object, client.Object, NullLogger<TranslationQuery>.Instance);

            var description = @"It was created by
            a scientist after
            years of horrific
            gene splicing and
            DNA engineering
            experiments.";

            // Act
            var translated = await query.Translate(Translation.Shakespeare, description);

            // Assert
            Assert.Equal("translated", translated);
            cache.Verify(c => c.TryGet("Shakespeare-It was created by a scientist after years of horrific gene splicing and DNA engineering experiments."), Times.Once);
            cache.Verify(c => c.Set("Shakespeare-It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.", translated), Times.Once);
        }
    }
}
