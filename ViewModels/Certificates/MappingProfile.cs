using System.Linq;
using AutoMapper;
using Domain;
using UseCases.Certificates.SaveCertificates;
using ViewModels.Certificates.Models;

namespace ViewModels.Certificates
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Certificate, CertificateModel>();
            CreateMap<Image, ImageModel>();

            CreateMap<CertificatesModel, SaveCertificatesCommand>();
            CreateMap<CertificateModel, CertificateDto>();
            CreateMap<ImageModel, ImageDto>();
        }
    }
}