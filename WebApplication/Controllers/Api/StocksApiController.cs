using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.Stocks.GetAllStocks;

namespace WebApplication.Controllers.Api;

[ApiController]
[Route("api/v1/stocks")]
public class StocksApiController
{
    private readonly IMediator mediator;

    public StocksApiController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<Stock>> GetAllStocks([FromQuery]GetAllStocksQuery query, CancellationToken cancellationToken)
    {
        return await mediator.Send(query, cancellationToken);
    }
}