using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Leaders.GetLeaders;

namespace WebApplication.Controllers.Api;

/// <summary>
/// Controller represent API for interaction with <see cref="SalesLeader"/> launentity.
/// </summary>
[ApiController]
[Route("api/v1/sales-leaders")]
[AllowAnonymous]
public class SalesLeaderApiController
{
    private readonly IMediator _mediator;

    public SalesLeaderApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns all sales leaders.
    /// </summary>
    [HttpGet]
    public async Task<IList<SalesLeader>> GetLeaders([FromQuery] GetLeadersQuery query, CancellationToken cancellationToken) =>
        await _mediator.Send(query, cancellationToken);
}