using System.Collections.Generic;

namespace Domain
{
    public record Category
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public IList<Product> Products { get; init; }

        public Image Icon { get; init; }
    }
}