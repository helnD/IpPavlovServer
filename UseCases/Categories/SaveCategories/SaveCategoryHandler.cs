using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCases.Certificates.SaveCertificates;

namespace UseCases.Categories.SaveCategories;

public class SaveCategoryHandler : AsyncRequestHandler<SaveCategoriesCommand>
{
    private readonly IDbContext _context;
    private readonly IImageApi _imageApi;
    private readonly IMapper _mapper;

    public SaveCategoryHandler(IMapper mapper, IDbContext context, IImageApi imageApi)
    {
        _mapper = mapper;
        _context = context;
        _imageApi = imageApi;
    }

    protected override async Task Handle(SaveCategoriesCommand request, CancellationToken cancellationToken)
    {
        var added = _mapper.Map<IEnumerable<Category>>(request.Added).ToList();
        await UploadImages(request.Added, added, cancellationToken);
        await _context.Categories.AddRangeAsync(added, cancellationToken);

        var updated = await GetFromDatabase(request.Updated, cancellationToken);
        MapCategories(request.Updated, updated);
        await UploadImages(request.Updated, updated, cancellationToken);
        _context.Categories.UpdateRange(updated);

        var removed = await GetFromDatabase(request.Removed, cancellationToken);
        _context.Categories.RemoveRange(removed);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<IEnumerable<Category>> GetFromDatabase(IEnumerable<CategoryDto> categories, CancellationToken cancellationToken)
    {
        var ids = categories.Select(cert => cert.Id).ToArray();
        return await _context.Categories
            .Where(category => ids.Contains(category.Id))
            .ToListAsync(cancellationToken);
    }

    private void MapCategories(IEnumerable<CategoryDto> source, IEnumerable<Category> target)
    {
        foreach (var category in target)
        {
            var sourceCategory = source.First(_ => _.Id == category.Id);
            _mapper.Map(sourceCategory, category);
        }
    }

    private async Task UploadImages(IEnumerable<CategoryDto> source, IEnumerable<Category> target, CancellationToken cancellationToken)
    {
        foreach (var sourceCategory in source)
        {
            if (!sourceCategory.Icon.IsUpdated)
            {
                continue;
            }
            var imageId = await _imageApi.UploadImageAsync(sourceCategory.Icon.Path, "Categories", cancellationToken);
            var imageFromDatabase = await _context.Images.FirstAsync(image => image.Id == imageId, cancellationToken);

            var targetCategories = target.First(category => category.Id == sourceCategory.Id);
            targetCategories.Icon = imageFromDatabase;
        }
    }
}