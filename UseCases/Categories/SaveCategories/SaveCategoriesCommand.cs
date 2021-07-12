using System.Collections.Generic;
using MediatR;

namespace UseCases.Categories.SaveCategories
{
    public class SaveCategoriesCommand : IRequest
    {
        /// <summary>
        /// Added certificates.
        /// </summary>
        public IEnumerable<CategoryDto> Added { get; init; }

        /// <summary>
        /// Removed certificates.
        /// </summary>
        public IEnumerable<CategoryDto> Removed { get; init; }

        /// <summary>
        /// Updated certificates.
        /// </summary>
        public IEnumerable<CategoryDto> Updated { get; init; }
    }
}