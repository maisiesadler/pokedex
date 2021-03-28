namespace Pokedex.Domain
{
    public record TranslatedPokemonInformation
    {
        public string Name { get; }
        public string Description { get; }
        public string Habitat { get; }
        public bool IsLegendary { get; }

        public TranslatedPokemonInformation(BasicPokemonInformation basicInfo, string description)
            => (Name, Description, Habitat, IsLegendary) = (basicInfo.Name, description, basicInfo.Habitat, basicInfo.IsLegendary);
    }
}