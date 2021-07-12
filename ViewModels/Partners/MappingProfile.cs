using AutoMapper;
using Domain;
using UseCases.Partners.SavePartnersCommand;
using ViewModels.Partners.Models;

namespace ViewModels.Partners
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Partner, PartnerModel>();

            CreateMap<PartnersModel, SavePartnersCommand>();
            CreateMap<PartnerModel, PartnerDto>();
        }
    }
}