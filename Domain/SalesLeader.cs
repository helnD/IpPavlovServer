
namespace Domain
{
    /// <summary>
    /// Represents one of current sales leaders.
    /// </summary>
    public record SalesLeader
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// A product that is a sales leader.
        /// </summary>
        public Product Product { get; init; }
    }
}