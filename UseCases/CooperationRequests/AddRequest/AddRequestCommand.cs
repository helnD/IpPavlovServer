using MediatR;

namespace UseCases.CooperationRequests.AddRequest
{
    public class AddRequestCommand : IRequest
    {
        public string Company { get; init; }

        public string Name { get; init; }

        public string Email { get; init; }
    }
}