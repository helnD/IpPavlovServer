using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UseCases.SalesRepresentatives.GetRepresentatives
{
    /// <summary>
    /// Handle get representatives query.
    /// </summary>
    public class GetRepresentativesHandler : IRequestHandler<GetRepresentativesQuery, IEnumerable<SalesRepresentative>>
    {
        private readonly IDbContext _context;

        public GetRepresentativesHandler(IDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handler for <see cref="GetRepresentativesQuery"/>.
        /// </summary>
        /// <param name="request"><see cref="GetRepresentativesQuery"/> request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of sales representatives.</returns>
        public async Task<IEnumerable<SalesRepresentative>> Handle(GetRepresentativesQuery request, CancellationToken cancellationToken)
        {
            return await _context.SalesRepresentatives.ToListAsync(cancellationToken);
        }
    }
}