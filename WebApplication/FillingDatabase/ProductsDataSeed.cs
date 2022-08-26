using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain;
using Infrastructure.Abstractions;
using Infrastructure.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UseCases.Resources.UploadImage;
using Image = Domain.Image;

namespace WebApplication.FillingDatabase;

/// <summary>
/// Seeding products data.
/// </summary>
public class ProductsDataSeed
{
    private readonly IExcelReader _excelReader;
    private readonly IDbContext _context;
    private readonly ILogger<ProductsDataSeed> _logger;
    private readonly global::Infrastructure.Abstractions.Unidecode _unidecode;
    private readonly IMediator _mediator;

    private const int ProductNumber = 0;
    private const int ProductCode = 1;
    private const int Description = 2;
    private const int Quantity = 3;
    private const int Price = 4;
    private const int ImageName = 10;
    private const int Producer = 11;
    private const int Category = 12;

    /// <summary>
    /// Constructor.
    /// </summary>
    public ProductsDataSeed(IExcelReader excelReader,
        IDbContext context,
        ILogger<ProductsDataSeed> logger,
        global::Infrastructure.Abstractions.Unidecode unidecode,
        IMediator mediator)
    {
        _excelReader = excelReader;
        _context = context;
        _logger = logger;
        _unidecode = unidecode;
        _mediator = mediator;
    }

    /// <summary>
    /// Seed products.
    /// </summary>
    /// <param name="pathToArchive">Path to archive with product images.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task SeedProducts(string pathToArchive, CancellationToken cancellationToken)
    {
        if (await _context.Products.AnyAsync(cancellationToken))
        {
            return;
        }

        await using var zipToOpen = new FileStream(pathToArchive, FileMode.Open);
        using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read);

        while (!_excelReader.IsEnd)
        {
            var row = _excelReader.NextRow();

            var number = row[ProductNumber];
            if (string.IsNullOrEmpty(number) || !int.TryParse(number, out var num))
            {
                continue;
            }

            var image = GetImage(archive, row);

            var product = await CreateProduct(image, row, cancellationToken);
            await _context.Products.AddAsync(product, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        var stockProducts = await _context.Products.Take(2).ToListAsync(cancellationToken);
        await _context.Stocks.AddAsync(new ()
        {
            RelatedProducts = stockProducts,
            Text = $"Купите один [{stockProducts[0].Id}:продукт] и получите [{stockProducts[1].Id}:второй] в подарок"
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Products initialization complete");
    }

    private Maybe<Stream> GetImage(ZipArchive archive, IList<string> row)
    {
        if (row.Count <= ImageName)
        {
            return Maybe<Stream>.None;
        }

        var sourceImageName = row[ImageName];
        if (string.IsNullOrWhiteSpace(sourceImageName))
        {
            return Maybe<Stream>.None;
        }

        var freeWhitespacesName = row[ImageName]
            .Replace("", "")
            .Replace(" ", "");

        var invalidChars = Path.GetInvalidFileNameChars();

        var invalidCharsRemoved = new string(freeWhitespacesName
            .Where(symbol => !invalidChars.Contains(symbol))
            .ToArray());

        var imageName = _unidecode(invalidCharsRemoved);
        var imageEntry = archive.GetEntry(imageName);
        var imageStream = imageEntry?.Open();

        if (imageStream is null)
        {
            return Maybe<Stream>.None;
        }

        return Maybe<Stream>.From(imageStream);
    }

    private async Task<Product> CreateProduct(Maybe<Stream> image, IList<string> row,
        CancellationToken cancellationToken)
    {
        var number = int.Parse(row[ProductNumber]);
        var code = int.Parse(row[ProductCode]);
        var quantity = int.Parse(row[Quantity]);
        var price = decimal.Parse(row[Price], CultureInfo.InvariantCulture);
        var description = row[Description];
        var categoryName = row[Category];
        var producerName = row[Producer];
        var imageName = row.Count <= ImageName ? Maybe<string>.None : Maybe<string>.From(row[ImageName]);

        var categoryFromDatabase = await _context.Categories
            .FirstOrDefaultAsync(category =>
                category.Name.ToUpper() == categoryName.ToUpper(), cancellationToken);

        if (categoryFromDatabase == default)
        {
            throw new ArgumentException($"Category with name = {categoryName} not found");
        }

        var producerFromDatabase = await _context.Partners
            .FirstOrDefaultAsync(producer =>
                producer.Name.ToUpper().Trim() == producerName.ToUpper().Trim(), cancellationToken);

        var productImage = Maybe<Image>.None;
        if (image.HasValue && imageName.HasValue)
        {
            var imageId = await _mediator.Send(new UploadImageCommand
            {
                Name = imageName.Value,
                Type = "Products",
                ImageStream = image.Value
            }, cancellationToken);

            productImage = await _context.Images.FindAsync(new object[] { imageId }, cancellationToken: cancellationToken);
            image.Value.Close();
        }

        if (image.HasNoValue && imageName.HasValue)
        {
            _logger.LogWarning("Image not found: {ImageName}", imageName.Value);
        }

        return new Product
        {
            DocumentId = number,
            Code = code,
            Quantity = quantity,
            Price = price,
            Unit = "шт",
            Description = description,
            Category = categoryFromDatabase,
            Producer = producerFromDatabase,
            Image = productImage.GetValueOrDefault()
        };
    }
}