using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Infrastructure.Abstractions;
using MediatR;

namespace UseCases.CooperationRequests.AddRequest;

public class AddRequestHandler : AsyncRequestHandler<AddRequestCommand>
{
    private readonly IDbContext _context;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;

    public AddRequestHandler(IDbContext context, IMapper mapper, IEmailSender emailSender)
    {
        _context = context;
        _mapper = mapper;
        _emailSender = emailSender;
    }

    protected override async Task Handle(AddRequestCommand request, CancellationToken cancellationToken)
    {
        var requestToAdd = _mapper.Map<CooperationRequest>(request);
        await _context.CooperationRequests.AddAsync(requestToAdd, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _emailSender.SendCooperationRequest(request.Name, request.Company, request.Email, cancellationToken);
    }
}