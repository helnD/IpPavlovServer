using AutoMapper;
using Domain;
using ViewModels.Partners.Models;

namespace ViewModels.Partners
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Partner, PartnerModel>();
        }
    }
}