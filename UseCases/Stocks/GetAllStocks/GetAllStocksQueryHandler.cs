using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UseCases.Stocks.GetAllStocks;

public class GetAllStocksQueryHandler : IRequestHandler<GetAllStocksQuery, IEnumerable<Stock>>
{
    private readonly IDbContext context;

    public GetAllStocksQueryHandler(IDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Stock>> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
    {
        var stocks = await context.Stocks.Include(stock => stock.RelatedProducts)
            .ThenInclude(product => product.Category)
            .ThenInclude(category => category.Icon)
            .Include(stock => stock.RelatedProducts)
            .ThenInclude(product => product.Image)
            .ToListAsync(cancellationToken);

        foreach (var product in stocks.SelectMany(stock => stock.RelatedProducts))
        {
            product.Image ??= product.Category.Icon;
        }

        return stocks;
    }
}