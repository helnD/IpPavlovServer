using Domain;

namespace UseCases.Products.GetProducts.Dtos
{
    /// <summary>
    /// Data Transfer Object for category.
    /// </summary>
    public class CategoryDto
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
        /// Icon of category.
        /// </summary>
        public Image Icon { get; init; }
    }
}