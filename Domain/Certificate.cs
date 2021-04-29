namespace Domain
{
    public record Certificate
    {
        public int Id { get; init; }

        public string Description { get; init; }

        public Image Image { get; init; }
    }
}