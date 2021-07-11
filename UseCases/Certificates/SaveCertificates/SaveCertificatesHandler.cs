using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UseCases.Certificates.SaveCertificates
{
    public class SaveCertificatesHandler : AsyncRequestHandler<SaveCertificatesCommand>
    {
        private readonly IDbContext _context;
        private readonly IImageApi _imageApi;
        private readonly IMapper _mapper;

        public SaveCertificatesHandler(IMapper mapper, IDbContext context, IImageApi imageApi)
        {
            _mapper = mapper;
            _context = context;
            _imageApi = imageApi;
        }

        protected override async Task Handle(SaveCertificatesCommand request, CancellationToken cancellationToken)
        {
            var added = _mapper.Map<IEnumerable<Certificate>>(request.Added).ToList();
            await UploadImages(request.Added, added, cancellationToken);
            await _context.Certificates.AddRangeAsync(added, cancellationToken);

            var updated = await GetFromDatabase(request.Updated, cancellationToken);
            MapCertificates(request.Updated, updated);
            await UploadImages(request.Updated, updated, cancellationToken);
            _context.Certificates.UpdateRange(updated);

            var removed = await GetFromDatabase(request.Removed, cancellationToken);
            _context.Certificates.RemoveRange(removed);

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<IEnumerable<Certificate>> GetFromDatabase(IEnumerable<CertificateDto> certificates, CancellationToken cancellationToken)
        {
            var ids = certificates.Select(cert => cert.Id).ToArray();
            return await _context.Certificates
                .Where(certificate => ids.Contains(certificate.Id))
                .ToListAsync(cancellationToken);
        }

        private void MapCertificates(IEnumerable<CertificateDto> source, IEnumerable<Certificate> target)
        {
            foreach (var certificate in target)
            {
                var sourceCertificate = source.First(_ => _.Id == certificate.Id);
                _mapper.Map(sourceCertificate, certificate);
            }
        }

        private async Task UploadImages(IEnumerable<CertificateDto> source, IEnumerable<Certificate> target, CancellationToken cancellationToken)
        {
            foreach (var sourceCertificate in source)
            {
                var imageId = await _imageApi.UploadImageAsync(sourceCertificate.Image.Path, "Certificates", cancellationToken);
                var imageFromDatabase = await _context.Images.FirstAsync(image => image.Id == imageId, cancellationToken);

                var targetCertificate = target.First(cert => cert.Id == sourceCertificate.Id);
                targetCertificate.Image = imageFromDatabase;
            }
        }
    }
}