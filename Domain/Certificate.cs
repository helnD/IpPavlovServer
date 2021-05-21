namespace Domain
{
    /// <summary>
    /// Certificate.
    /// </summary>
    public record Certificate
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Certificate description.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Image of certificate.
        /// </summary>
        public Image Image { get; init; }
    }
}