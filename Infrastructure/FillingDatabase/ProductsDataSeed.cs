﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.FillingDatabase
{
    /// <summary>
    /// Seeding products data.
    /// </summary>
    public class ProductsDataSeed
    {
        private readonly IExcelReader _excelReader;
        private readonly IDbContext _context;
        private readonly ILogger<ProductsDataSeed> _logger;
        private readonly ImagesSettings _imagesSettings;
        private readonly IList<string> _availableImages;

        private const int ProductNumber = 0;
        private const int ProductCode = 1;
        private const int Description = 2;
        private const int Quantity = 3;
        private const int Price = 4;
        private const int Category = 10;
        private const int Producer = 11;

        public ProductsDataSeed(IExcelReader excelReader, IDbContext context, IOptions<ImagesSettings> imagesSettings,
            ILogger<ProductsDataSeed> logger)
        {
            _excelReader = excelReader;
            _context = context;
            _logger = logger;
            _imagesSettings = imagesSettings.Value;

            var imagesFolder = Path.Combine(imagesSettings.Value.Root, imagesSettings.Value.Products);
            _availableImages = new DirectoryInfo(imagesFolder).GetFiles().Select(file => file.FullName).ToList();
        }

        /// <summary>
        /// Seed products.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task SeedProducts(CancellationToken cancellationToken)
        {
            if (await _context.Products.AnyAsync(cancellationToken))
            {
                return;
            }

            var leadersCount = 0;

            while (!_excelReader.IsEnd)
            {
                var row = _excelReader.NextRow();

                var number = row[ProductNumber];
                if (string.IsNullOrEmpty(number) || !int.TryParse(number, out var num))
                {
                    continue;
                }

                var product = await CreateProduct(row, cancellationToken);
                await _context.Products.AddAsync(product, cancellationToken);

                if (leadersCount < 2)
                {
                    await _context.Leaders.AddAsync(new SalesLeader { Product = product }, cancellationToken);
                    leadersCount++;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Products initialization complete");
        }

        private async Task<Product> CreateProduct(IList<string> row, CancellationToken cancellationToken)
        {
            var number = int.Parse(row[ProductNumber]);
            var code = int.Parse(row[ProductCode]);
            var quantity = int.Parse(row[Quantity]);
            var price = decimal.Parse(row[Price], CultureInfo.InvariantCulture);
            var description = row[Description];
            var categoryName = row[Category];
            var producerName = row[Producer];

            var categoryFromDatabase = await _context.Categories
                .FirstOrDefaultAsync(category => category.Name == categoryName, cancellationToken);

            if (categoryFromDatabase == default)
            {
                throw new ArgumentException($"Category with name = {categoryName} not found");
            }

            var producerFromDatabase = await _context.Partners
                .FirstOrDefaultAsync(producer => producer.Name == producerName, cancellationToken);

            if (producerFromDatabase == default)
            {
                throw new ArgumentException($"Producer with name = {producerName} not found");
            }

            var pathToImage = Path.Combine(_imagesSettings.Root, _imagesSettings.Products,
                _availableImages[number % _availableImages.Count]);

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
                Image = new Image
                {
                    Path = pathToImage
                }
            };
        }
    }
}