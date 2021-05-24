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
    /// Partners seeding test data.
    /// </summary>
    public class PartnersDataSeed
    {
        private readonly IDbContext _context;
        private readonly DatabaseInitialization _initializationSettings;
        private readonly ILogger<PartnersDataSeed> _logger;
        private readonly ImagesSettings _imagesSettings;

        private const string Partners = "partners";
        private const string Partner = "partner";
        private const string Description = "description";

        public PartnersDataSeed(IDbContext context, IOptions<DatabaseInitialization> initializationSettings,
            IOptions<ImagesSettings> imagesSettings, ILogger<PartnersDataSeed> logger)
        {
            _context = context;
            _initializationSettings = initializationSettings.Value;
            _logger = logger;
            _imagesSettings = imagesSettings.Value;
        }

        /// <summary>
        /// Seed partners.
        /// </summary>
        public async Task SeedPartners(CancellationToken cancellationToken)
        {
            if (await _context.Partners.AnyAsync(cancellationToken))
            {
                return;
            }

            var pathToAssembly = Path.GetDirectoryName(GetType().Assembly.Location);
            var partnersFile = Path.Combine(pathToAssembly, _initializationSettings.PartnersFile);

            var partnersDocument = XDocument.Load(new FileStream(partnersFile, FileMode.Open),
                LoadOptions.None);

            var partnersNode = partnersDocument.Descendants(Partners).First();

            if (partnersNode == null)
            {
                throw new InvalidOperationException(
                    "You should create file with partners before start database initialization");
            }


            foreach (var partnerNode in partnersDocument.Descendants(Partner))
            {
                await _context.Partners.AddAsync(CreatePartner(partnerNode), cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Partners initialized");
        }

        private Partner CreatePartner(XElement partnerNode)
        {
            var name = partnerNode.Attribute("name")?.Value;
            var iconName = partnerNode.Attribute("icon")?.Value;
            var description = partnerNode.Descendants(Description).First().Value.Trim();

            return new Partner
            {
                Name = name,
                Description = description,
                Image = new Image
                {
                    Path = Path.Combine(_imagesSettings.Root, _imagesSettings.Partners, iconName)
                }
            };
        }
    }
}