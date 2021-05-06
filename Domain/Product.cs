using System;

namespace Domain
{
    public record Product
    {
        public int Id { get; init; }

        public string Description { get; init; }

        public int Quantity { get; init; }

        public decimal Price { get; init; }

        public Category Category { get; init; }

        public Partner Producer { get; init; }

        public Image Image { get; init; }
    }
}
