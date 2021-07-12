using AutoMapper;
using Domain;
using UseCases.Partners.SavePartnersCommand;

namespace UseCases.Partners
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PartnerDto, Partner>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());;
        }
    }
}