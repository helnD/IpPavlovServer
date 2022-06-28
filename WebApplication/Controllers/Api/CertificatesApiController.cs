using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.Certificates.GetCertificates;

namespace WebApplication.Controllers.Api;

/// <summary>
/// Controller for certificates.
/// </summary>
[ApiController]
[Route("api/v1/certificates")]
public class CertificatesApiController
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mediator">MediatR object.</param>
    public CertificatesApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns all certificates.
    /// </summary>
    /// <param name="query">Request for all certificates.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet]
    public async Task<IEnumerable<Certificate>> GetAllCategories([FromQuery]GetCertificatesQuery query, CancellationToken cancellationToken)
    {
        return await _mediator.Send(query, cancellationToken);
    }
}