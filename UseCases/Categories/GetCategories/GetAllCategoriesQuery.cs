using System.Collections.Generic;
using Domain;
using MediatR;

namespace UseCases.Categories.GetCategories
{
    /// <summary>
    /// Get all categories.
    /// </summary>
    public class GetAllCategoriesQuery : IRequest<IEnumerable<Category>>
    {
        public string Name { get; init; }
    }
}