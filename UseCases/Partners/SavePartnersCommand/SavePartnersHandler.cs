using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UseCases.Partners.SavePartnersCommand
{
    public class SavePartnersHandler : AsyncRequestHandler<SavePartnersCommand>
    {
        private readonly IDbContext _context;
        private readonly IImageApi _imageApi;
        private readonly IMapper _mapper;

        public SavePartnersHandler(IMapper mapper, IDbContext context, IImageApi imageApi)
        {
            _mapper = mapper;
            _context = context;
            _imageApi = imageApi;
        }

        protected override async Task Handle(SavePartnersCommand request, CancellationToken cancellationToken)
        {
            var added = _mapper.Map<IEnumerable<Partner>>(request.Added).ToList();
            await UploadImages(request.Added, added, cancellationToken);
            await _context.Partners.AddRangeAsync(added, cancellationToken);

            var updated = await GetFromDatabase(request.Updated, cancellationToken);
            MapCategories(request.Updated, updated);
            await UploadImages(request.Updated, updated, cancellationToken);
            _context.Partners.UpdateRange(updated);

            var removed = await GetFromDatabase(request.Removed, cancellationToken);
            _context.Partners.RemoveRange(removed);

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<IEnumerable<Partner>> GetFromDatabase(IEnumerable<PartnerDto> partners, CancellationToken cancellationToken)
        {
            var ids = partners.Select(cert => cert.Id).ToArray();
            return await _context.Partners
                .Where(partner => ids.Contains(partner.Id))
                .ToListAsync(cancellationToken);
        }

        private void MapCategories(IEnumerable<PartnerDto> source, IEnumerable<Partner> target)
        {
            foreach (var partner in target)
            {
                var sourcePartner = source.First(_ => _.Id == partner.Id);
                _mapper.Map(sourcePartner, partner);
            }
        }

        private async Task UploadImages(IEnumerable<PartnerDto> source, IEnumerable<Partner> target, CancellationToken cancellationToken)
        {
            foreach (var sourcePartner in source)
            {
                if (!sourcePartner.Image.IsUpdated)
                {
                    continue;
                }
                var imageId = await _imageApi.UploadImageAsync(sourcePartner.Image.Path, "Categories", cancellationToken);
                var imageFromDatabase = await _context.Images.FirstAsync(image => image.Id == imageId, cancellationToken);

                var targetCategories = target.First(category => category.Id == sourcePartner.Id);
                targetCategories.Image = imageFromDatabase;
            }
        }
    }
}