
namespace Domain
{
    public record SalesLeader
    {
        public int Id { get; init; }

        public Product Product { get; init; }
    }
}