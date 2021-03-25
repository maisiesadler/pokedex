using System.Text.Json.Serialization;

namespace Pokedex.Domain.Queries
{
    public record FlavorTextEntry
    {
        [JsonPropertyName("flavor_text")]
        public string FlavorText { get; }

        public Resource Language { get; }

        public FlavorTextEntry(string flavorText, Resource language) => (FlavorText, Language) = (flavorText, language);
    }
}
