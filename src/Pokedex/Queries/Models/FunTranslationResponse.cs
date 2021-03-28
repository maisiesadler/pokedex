namespace Pokedex.Queries.Models
{
    public record FunTranslationResponse
    {
        public FunTranslationResponseContents Contents { get; set; }

        public FunTranslationResponse(FunTranslationResponseContents contents)
            => (Contents) = (contents);
    }
}
