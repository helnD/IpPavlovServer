using System.Collections;

namespace Domain
{
    public record SalesLeader
    {
        public int Id { get; }

        public Product Product { get; init; }
    }
}