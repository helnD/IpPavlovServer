using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UseCases.Categories.GetCategories;

/// <summary>
/// Handle <see cref="GetAllCategoriesQuery"/>.
/// </summary>
public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
{
    private readonly IDbContext _context;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="context">Database context.</param>
    public GetAllCategoriesHandler(IDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handler for <see cref="GetAllCategoriesQuery"/>.
    /// </summary>
    /// <param name="request"><see cref="GetAllCategoriesQuery"/> request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All categories.</returns>
    public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Category> categories = _context.Categories
            .Include(category => category.Icon);

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var upperName = request.Name.ToUpper();
            categories = categories.Where(category => category.Name.ToUpper().Contains(upperName));
        }

        return await categories.ToListAsync(cancellationToken);
    }
}