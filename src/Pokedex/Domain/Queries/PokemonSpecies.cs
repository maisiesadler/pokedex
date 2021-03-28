using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pokedex.Domain.Queries
{
    public record PokemonSpecies
    {
        public string Name { get; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; }

        [JsonPropertyName("flavor_text_entries")]
        public List<FlavorTextEntry> FlavorTextEntries { get; }

        public Resource Habitat { get; }

        public PokemonSpecies(string name, bool isLegendary, List<FlavorTextEntry> flavorTextEntries, Resource habitat)
            => (Name, IsLegendary, FlavorTextEntries, Habitat) = (name, isLegendary, flavorTextEntries, habitat);
    }
}
