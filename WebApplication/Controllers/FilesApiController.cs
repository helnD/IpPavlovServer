﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.Resources.GetPriceList;

namespace WebApplication.Controllers
{
    /// <summary>
    /// Controller for downloading/uploading files.
    /// </summary>
    [ApiController]
    [Route("api/v1/files")]
    public class FilesApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FilesApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("price-list")]
        public async Task<FileResult> GetPriceList(CancellationToken cancellationToken)
        {
            var stream = await _mediator.Send(new GetPriceListQuery(), cancellationToken);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "price-list.xlsx");
        }
    }
}