using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.CooperationRequests.AddRequest;

namespace WebApplication.Controllers.Api;

[ApiController]
[Route("api/v1/cooperation-requests")]
public class CooperationRequestsApiController
{
    private readonly IMediator _mediator;

    public CooperationRequestsApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task AddRequest(AddRequestCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
    }
}