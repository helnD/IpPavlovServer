using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.FillingDatabase
{
    /// <summary>
    /// Class for seeding data from data seed xml-configuration.
    /// </summary>
    /// <typeparam name="T">Type of table entity.</typeparam>
    public class XmlSeeder<T> where T : class, new()
    {
        private readonly IDbContext _context;
        private readonly ILogger<XmlSeeder<T>> _logger;

        public XmlSeeder(IDbContext context, ILogger<XmlSeeder<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Seed data from XML file.
        /// </summary>
        /// <param name="fileName">XML filename.</param>
        /// <param name="createEntity">Object creation logic.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task SeedData(string fileName, Func<XElement, T> createEntity, CancellationToken cancellationToken)
        {
            if (await _context.Entity<T>().AnyAsync(cancellationToken))
            {
                return;
            }

            var pathToAssembly = Path.GetDirectoryName(GetType().Assembly.Location);
            var file = Path.Combine(pathToAssembly, fileName);

            var xmlDocument = XDocument.Load(new FileStream(file, FileMode.Open),
                LoadOptions.None);

            var collectionNode = xmlDocument.Descendants()
                .FirstOrDefault(node => node.Attribute("type") is not null);
            var className = typeof(T).Name;
            if (collectionNode == null)
            {
                throw new InvalidOperationException(
                    $"You should create file with {className} entity before start database initialization");
            }

            foreach (var partnerNode in collectionNode.Descendants())
            {
                await _context.Entity<T>().AddAsync(createEntity(partnerNode), cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Table for {Name} entity initialized", className);
        }
    }
}