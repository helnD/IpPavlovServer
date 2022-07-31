using AutoMapper;
using Domain;
using UseCases.Certificates.SaveCertificates;

namespace UseCases.Certificates;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CertificateDto, Certificate>()
            .ForMember(dest => dest.Image, opt => opt.Ignore());
        CreateMap<ImageDto, Image>();
    }
}