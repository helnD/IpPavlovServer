using System.Collections.Generic;
using Domain;
using MediatR;

namespace UseCases.GetCategories
{
    /// <summary>
    /// Get all categories.
    /// </summary>
    public class GetAllCategoriesQuery : IRequest<IEnumerable<Category>>
    {

    }
}