using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pokedex.Domain.Queries
{
    public record PokemonSpecies
    {
        public string Name { get; }

        [JsonPropertyName("flavor_text_entries")]
        public List<FlavorTextEntry> FlavorTextEntries { get; }

        public Resource Habitat { get; }

        public PokemonSpecies(string name, List<FlavorTextEntry> flavorTextEntries, Resource habitat)
            => (Name, FlavorTextEntries, Habitat) = (name, flavorTextEntries, habitat);
    }
}
