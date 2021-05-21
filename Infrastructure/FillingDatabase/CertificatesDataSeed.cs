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
    /// Certificates seeding test data.
    /// </summary>
    public class CertificatesDataSeed
    {
        private readonly IDbContext _context;
        private readonly DatabaseInitialization _initializationSettings;
        private readonly ILogger<CertificatesDataSeed> _logger;
        private readonly ImagesSettings _imagesSettings;

        private const string Certificates = "certificates";
        private const string Certificate = "certificate";
        private const string Description = "description";

        public CertificatesDataSeed(IDbContext context, IOptions<DatabaseInitialization> initializationSettings,
            IOptions<ImagesSettings> imagesSettings, ILogger<CertificatesDataSeed> logger)
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
            var certificatesFile = Path.Combine(pathToAssembly, _initializationSettings.CertificatesFile);

            var certificatesDocument = XDocument.Load(new FileStream(certificatesFile, FileMode.Open),
                LoadOptions.None);

            var certificatesNode = certificatesDocument.Descendants(Certificates).First();

            if (certificatesNode == null)
            {
                throw new InvalidOperationException(
                    "You should create file with certificates before start database initialization");
            }


            foreach (var certificateNode in certificatesDocument.Descendants(Certificate))
            {
                await _context.Certificates.AddAsync(CreateCertificate(certificateNode), cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Certificates initialized");
        }

        private Certificate CreateCertificate(XElement certificateNode)
        {
            var imageName = certificateNode.Attribute("image")?.Value;
            var description = certificateNode.Descendants(Description).First().Value;

            return new Certificate
            {
                Description = description,
                Image = new Image
                {
                    Path = Path.Combine(_imagesSettings.Root, _imagesSettings.Certificates, imageName)
                }
            };
        }
    }
}