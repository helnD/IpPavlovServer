using System.Collections.Generic;

namespace Domain
{
    public class Stock
    {
        public int Id { get; init; }

        public string Text { get; init; }

        public IEnumerable<Product> RelatedProducts { get; init; }
    }
}