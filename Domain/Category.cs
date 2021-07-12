using System.Collections.Generic;

namespace Domain
{
    /// <summary>
    /// Category of product.
    /// </summary>
    public record Category
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Category name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Name for routing transition.
        /// </summary>
        public string RouteName { get; init; }

        /// <summary>
        /// Products list.
        /// </summary>
        public IList<Product> Products { get; init; }

        /// <summary>
        /// Icon of category.
        /// </summary>
        public Image Icon { get; set; }
    }
}