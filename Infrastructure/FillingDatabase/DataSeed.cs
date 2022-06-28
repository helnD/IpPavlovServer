using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Domain;
using Infrastructure.Abstractions;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.FillingDatabase;

/// <summary>
/// Class for database initial seed.
/// </summary>
public class DataSeed
{
    private readonly XmlSeederFacade _xmlSeederFacade;
    private readonly ProductsDataSeed _productsDataSeed;
    private readonly DatabaseInitialization _initializationSettings;
    private readonly ImagesSettings _imagesSettings;
    private readonly IImageResizer _imageResizer;
    private readonly Unidecode _unidecode;

    private const short Threshold = 200;

    public DataSeed(XmlSeederFacade xmlSeederFacade, IOptions<DatabaseInitialization> initializationSettings,
        IOptions<ImagesSettings> imagesSettings, Unidecode unidecode, ProductsDataSeed productsDataSeed, IImageResizer imageResizer)
    {
        _xmlSeederFacade = xmlSeederFacade;
        _initializationSettings = initializationSettings.Value;
        _imagesSettings = imagesSettings.Value;
        _unidecode = unidecode;
        _productsDataSeed = productsDataSeed;
        _imageResizer = imageResizer;
    }

    /// <summary>
    /// Seed categories.
    /// </summary>
    public async Task SeedTestDatabase(CancellationToken cancellationToken)
    {
        await _xmlSeederFacade.CategoriesSeeder.SeedData(_initializationSettings.CategoriesFile, CreateCategoryAsync,
            cancellationToken);
        await _xmlSeederFacade.CertificatesSeeder.SeedData(_initializationSettings.CertificatesFile, CreateCertificate,
            cancellationToken);
        await _xmlSeederFacade.PartnersSeeder.SeedData(_initializationSettings.PartnersFile, CreatePartnerAsync,
            cancellationToken);
        await _xmlSeederFacade.SalesRepresentativesSeeder.SeedData(_initializationSettings.RepresentativesFile,
            CreateRepresentativeAsync, cancellationToken);
        await _productsDataSeed.SeedProducts(cancellationToken);
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

    private async Task<Certificate> CreateCertificate(XElement certificateNode)
    {
        const string descriptionName = "description";

        var imageName = certificateNode.Attribute("image")?.Value;
        var description = certificateNode.Descendants(descriptionName).First().Value.Trim();

        await CreateMiniAsync(Path.Combine(_imagesSettings.Root, _imagesSettings.Certificates), imageName);

        return new Certificate
        {
            Description = description,
            Image = new Image
            {
                Path = Path.Combine(_imagesSettings.Root, _imagesSettings.Certificates, imageName)
            }
        };
    }

    private async Task<Partner> CreatePartnerAsync(XElement partnerNode)
    {
        const string descriptionName = "description";

        var name = partnerNode.Attribute("name")?.Value;
        var iconName = partnerNode.Attribute("icon")?.Value;
        var link = partnerNode.Attribute("link")?.Value;
        var description = partnerNode.Descendants(descriptionName).First().Value.Trim();

        await CreateMiniAsync(Path.Combine(_imagesSettings.Root, _imagesSettings.Partners), iconName);

        var partner = new Partner
        {
            Name = name,
            Description = description,
            Link = link,
            Image = new Image
            {
                Path = Path.Combine(_imagesSettings.Root, _imagesSettings.Partners, iconName)
            }
        };

        return partner;
    }

    private Task<SalesRepresentative> CreateRepresentativeAsync(XElement partnerNode)
    {
        var firstName = partnerNode.Attribute("firstName")?.Value;
        var lastName = partnerNode.Attribute("lastName")?.Value;
        var middleName = partnerNode.Attribute("middleName")?.Value;
        var phone = partnerNode.Attribute("phone")?.Value;
        var region = partnerNode.Attribute("region")?.Value;
        var start = partnerNode.Attribute("startOfWork")?.Value;
        var end = partnerNode.Attribute("endOfWork")?.Value;

        var salesRepresentative = new SalesRepresentative
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            Phone = phone,
            Region = region,
            StartOfWork = TimeSpan.Parse(start),
            EndOfWork = TimeSpan.Parse(end)
        };

        return Task.FromResult(salesRepresentative);
    }

    private async Task CreateMiniAsync(string root, string name)
    {
        await using var image = new FileStream(Path.Combine(root, name), FileMode.Open);
        await using var resizedImage = await _imageResizer.Reduce(image, Threshold, default);

        var miniFilename = Path.Combine(root, _imagesSettings.MiniPrefix + name);
        await using var mimiImage = File.Create(miniFilename);
        await resizedImage.CopyToAsync(mimiImage);
    }
}