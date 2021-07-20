using AutoMapper;
using Domain;
using UseCases.CooperationRequests.AddRequest;

namespace UseCases.CooperationRequests
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddRequestCommand, CooperationRequest>();
        }
    }
}