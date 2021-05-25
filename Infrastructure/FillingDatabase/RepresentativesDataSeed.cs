using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Domain;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.FillingDatabase
{
    public class RepresentativesDataSeed
    {
        private readonly DatabaseInitialization _initializationSettings;
        private readonly XmlSeeder<SalesRepresentative> _seeder;

        public RepresentativesDataSeed(XmlSeeder<SalesRepresentative> seeder,
            IOptions<DatabaseInitialization> initializationSettings)
        {
            _seeder = seeder;
            _initializationSettings = initializationSettings.Value;
        }

        /// <summary>
        /// Seed partners.
        /// </summary>
        public async Task SeedPartners(CancellationToken cancellationToken)
        {
            await _seeder.SeedData(_initializationSettings.RepresentativesFile, CreateRepresentative, cancellationToken);
        }

        private SalesRepresentative CreateRepresentative(XElement partnerNode)
        {
            var firstName = partnerNode.Attribute("firstName")?.Value;
            var lastName = partnerNode.Attribute("lastName")?.Value;
            var middleName = partnerNode.Attribute("middleName")?.Value;
            var phone = partnerNode.Attribute("phone")?.Value;
            var region = partnerNode.Attribute("region")?.Value;
            var start = partnerNode.Attribute("startOfWork")?.Value;
            var end = partnerNode.Attribute("endOfWork")?.Value;

            return new SalesRepresentative
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                Phone = phone,
                Region = region,
                StartOfWork = TimeSpan.Parse(start),
                EndOfWork = TimeSpan.Parse(end)
            };
        }
    }
}