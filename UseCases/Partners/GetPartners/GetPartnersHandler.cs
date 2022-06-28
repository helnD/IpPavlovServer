using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UseCases.Partners.GetPartners;

/// <summary>
/// Get all partners query handler.
/// </summary>
public class GetPartnersHandler : IRequestHandler<GetPartnersQuery, IEnumerable<Partner>>
{
    private readonly IDbContext _context;

    public GetPartnersHandler(IDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Query handler.
    /// </summary>
    /// <param name="request"><see cref="GetPartnersQuery"/> request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All partners.</returns>
    public async Task<IEnumerable<Partner>> Handle(GetPartnersQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Partners.Include(partner => partner.Image)
            .ToListAsync(cancellationToken);

        return result;
    }
}