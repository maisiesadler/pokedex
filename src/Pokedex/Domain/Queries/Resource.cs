namespace Pokedex.Domain.Queries
{
    public record Resource
    {
        public string Name { get; }
        public string Url { get; }

        public Resource(string name, string url) => (Name, Url) = (name, url);
    }
}
