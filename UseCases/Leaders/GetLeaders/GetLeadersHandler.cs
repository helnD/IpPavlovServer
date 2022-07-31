using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UseCases.Leaders.GetLeaders;

public class GetLeadersHandler : IRequestHandler<GetLeadersQuery, IList<SalesLeader>>
{
    private readonly IDbContext _context;

    public GetLeadersHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<IList<SalesLeader>> Handle(GetLeadersQuery request, CancellationToken cancellationToken)
    {
        var leaders = await IncludeAll().ToListAsync(cancellationToken);
        return leaders;
    }

    private IQueryable<SalesLeader> IncludeAll() => _context.Leaders
        .Include(leader => leader.Product)
        .ThenInclude(product => product.Category)
        .ThenInclude(category => category.Icon)
        .Include(leader => leader.Product)
        .ThenInclude(product => product.Producer)
        .ThenInclude(producer => producer.Image)
        .Include(leader => leader.Product)
        .ThenInclude(product => product.Image);
}