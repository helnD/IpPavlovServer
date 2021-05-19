using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UseCases.GetCategories
{
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
        public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .Include(category => category.Icon)
                .ToListAsync(cancellationToken);

            return categories;
        }
    }
}