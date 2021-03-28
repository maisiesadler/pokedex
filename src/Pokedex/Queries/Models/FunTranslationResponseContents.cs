namespace Pokedex.Queries.Models
{
    public record FunTranslationResponseContents
    {
        public string Translated { get; set; }

        public FunTranslationResponseContents(string translated)
            => (Translated) = (translated); 
    }
}
