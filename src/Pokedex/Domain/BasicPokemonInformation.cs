namespace Pokedex.Domain
{
    public record BasicPokemonInformation
    {
        public string Name { get; }
        public string Description { get; }
        public string Habitat { get; }
        public bool IsLegendary { get; }

        public BasicPokemonInformation(string name, string description, string habitat, bool isLegendary)
            => (Name, Description, Habitat, IsLegendary) = (name, description, habitat, isLegendary);
    }
}
