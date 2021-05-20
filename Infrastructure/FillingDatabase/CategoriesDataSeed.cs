using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Domain;
using Infrastructure.Abstractions;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.FillingDatabase
{
    /// <summary>
    /// Functionality for categories data seed.
    /// </summary>
    public class CategoriesDataSeed
    {
        private readonly IDbContext _context;
        private readonly DatabaseInitialization _initializationSettings;
        private readonly ILogger<CategoriesDataSeed> _logger;
        private readonly ImagesSettings _imagesSettings;

        private const string Categories = "categories";
        private const string Category = "category";

        public CategoriesDataSeed(IDbContext context, IOptions<DatabaseInitialization> initializationSettings,
            IOptions<ImagesSettings> imagesSettings, ILogger<CategoriesDataSeed> logger)
        {
            _context = context;
            _initializationSettings = initializationSettings.Value;
            _logger = logger;
            _imagesSettings = imagesSettings.Value;
        }

        /// <summary>
        /// Seed categories.
        /// </summary>
        public async Task SeedCategories(CancellationToken cancellationToken)
        {
            if (await _context.Categories.AnyAsync(cancellationToken))
            {
                return;
            }

            var pathToAssembly = Path.GetDirectoryName(GetType().Assembly.Location);
            var categoriesFile = Path.Combine(pathToAssembly, _initializationSettings.CategoriesFile);

            var categoriesDocument = XDocument.Load(new FileStream(categoriesFile, FileMode.Open),
                LoadOptions.None);

            var categoriesNode = categoriesDocument.Descendants(Categories).First();

            if (categoriesNode == null)
            {
                throw new InvalidOperationException(
                    "You should create file with categories before start database initialization");
            }


            foreach (var categoryNode in categoriesDocument.Descendants(Category))
            {
                await _context.Categories.AddAsync(CreateCategory(categoryNode), cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Categories initialized");
        }

        private Category CreateCategory(XElement categoryNode)
        {
            var name = categoryNode.Value;
            var iconName = categoryNode.Attribute("icon")?.Value;

            return new Category
            {
                Name = name,
                Icon = new Image
                {
                    Path = Path.Combine(_imagesSettings.Root, _imagesSettings.Categories, iconName)
                }
            };
        }
    }
}