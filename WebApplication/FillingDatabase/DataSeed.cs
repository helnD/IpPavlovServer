using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CSharpFunctionalExtensions;
using Domain;
using Infrastructure.Abstractions;
using Infrastructure.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace WebApplication.FillingDatabase;

/// <summary>
/// Class for database initial seed.
/// </summary>
public class DataSeed
{
    private readonly XmlSeederFacade _xmlSeederFacade;
    private readonly DatabaseInitialization _initializationSettings;
    private readonly ImagesSettings _imagesSettings;
    private readonly IImageResizer _imageResizer;
    private readonly Infrastructure.Abstractions.Unidecode _unidecode;
    private readonly IDbContext _dbContext;

    private const short Threshold = 200;
    private const string DealIconName = "deal.png";

    public DataSeed(XmlSeederFacade xmlSeederFacade,
        IOptions<DatabaseInitialization> initializationSettings,
        IOptions<ImagesSettings> imagesSettings,
        Infrastructure.Abstractions.Unidecode unidecode,
        IImageResizer imageResizer,
        IDbContext dbContext)
    {
        _xmlSeederFacade = xmlSeederFacade;
        _initializationSettings = initializationSettings.Value;
        _imagesSettings = imagesSettings.Value;
        _unidecode = unidecode;
        _imageResizer = imageResizer;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Seed database with initial data.
    /// </summary>
    public async Task SeedInitialDatabase(CancellationToken cancellationToken)
    {
        await _xmlSeederFacade.CategoriesSeeder.SeedData(_initializationSettings.CategoriesFile, CreateCategoryAsync,
            cancellationToken);
        await _xmlSeederFacade.PartnersSeeder.SeedData(_initializationSettings.PartnersFile, CreatePartnerAsync,
            cancellationToken);
    }

    private Task<Category> CreateCategoryAsync(XElement categoryNode)
    {
        var name = categoryNode.Value;
        var iconName = categoryNode.Attribute("icon")?.Value;
        var routeName = Regex.Replace(_unidecode(name), @"[',.-_]", "")
            .Replace(' ', '_');

        var category = new Category
        {
            Name = name,
            RouteName = routeName,
            Icon = new Image
            {
                Path = Path.Combine(_imagesSettings.Root, _imagesSettings.Categories, iconName)
            }
        };

        return Task.FromResult(category);
    }

    private async Task<Partner> CreatePartnerAsync(XElement partnerNode)
    {
        var name = partnerNode.Attribute("name")?.Value;
        var iconName = partnerNode.Attribute("icon")?.Value;
        var link = partnerNode.Attribute("link")?.Value;

        if (iconName is not null)
        {
            await CreateMiniAsync(Path.Combine(_imagesSettings.Root, _imagesSettings.Partners), iconName);
        }

        var partner = new Partner
        {
            Name = name,
            Link = link,
            Image = iconName is null
                ? await GetOrCreateDealIconAsync()
                : new Image
                {
                    Path = Path.Combine(_imagesSettings.Root, _imagesSettings.Partners, iconName)
                }
        };

        return partner;
    }

    private async Task CreateMiniAsync(string root, string name)
    {
        await using var image = new FileStream(Path.Combine(root, name), FileMode.Open);
        await using var resizedImage = await _imageResizer.Reduce(image, Threshold, default);

        var miniFilename = Path.Combine(root, _imagesSettings.MiniPrefix + name);
        await using var mimiImage = File.Create(miniFilename);
        await resizedImage.CopyToAsync(mimiImage);
    }

    private async Task<Image> GetOrCreateDealIconAsync()
    {
        var icon = await _dbContext.Images.FirstOrDefaultAsync(image => image.Path.Contains(DealIconName));
        if (icon != null)
        {
            return icon;
        }

        var root = Path.Combine(_imagesSettings.Root, _imagesSettings.Partners);
        var imagePath = Path.Combine(root, DealIconName);

        await using var image = new FileStream(imagePath, FileMode.Open);
        await using var resizedImage = await _imageResizer.Reduce(image, Threshold, default);

        var miniFilename = Path.Combine(root, _imagesSettings.MiniPrefix + DealIconName);
        await using var mimiImage = File.Create(miniFilename);
        await resizedImage.CopyToAsync(mimiImage);

        return new Image
        {
            Path = imagePath
        };
    }
}