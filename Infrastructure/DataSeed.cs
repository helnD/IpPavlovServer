using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Domain;
using Infrastructure.Abstractions;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
    /// <summary>
    /// Class for database initial seed.
    /// </summary>
    public class DataSeed
    {
        private readonly IDbContext _context;
        private readonly DatabaseInitialization _initializationSettings;
        private readonly ILogger<DataSeed> _logger;

        private const string Categories = "categories";
        private const string Category = "category";

        public DataSeed(IDbContext context, IOptions<DatabaseInitialization> initializationSettings)
        {
            _context = context;
            _initializationSettings = initializationSettings.Value;
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

            var categoriesDocument = new XmlDocument();
            categoriesDocument.Load(categoriesFile);

            var categoriesNode = categoriesDocument[Categories];

            if (categoriesNode == null)
            {
                throw new InvalidOperationException(
                    "You should create file with categories before start database initialization");
            }

            var categoriesReader = new XmlNodeReader(categoriesNode);

            while (categoriesReader.ReadToDescendant(Category))
            {
                var category = await categoriesReader.ReadElementContentAsStringAsync();
                await _context.Categories.AddAsync(new Category
                {
                    Name = category
                }, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Categories initialized");
        }
    }
}