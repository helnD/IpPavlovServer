using System.Collections.Generic;

namespace Domain
{
    public class Partner
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Description { get; init; }

        public IList<Product> Products { get; init; }

        public Image Image { get; init; }
    }
}