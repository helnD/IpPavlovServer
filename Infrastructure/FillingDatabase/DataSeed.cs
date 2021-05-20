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
    /// Class for database initial seed.
    /// </summary>
    public class DataSeed
    {
        private readonly CategoriesDataSeed _categoriesDataSeed;

        public DataSeed(CategoriesDataSeed categoriesDataSeed)
        {
            _categoriesDataSeed = categoriesDataSeed;
        }

        /// <summary>
        /// Seed categories.
        /// </summary>
        public async Task SeedTestDatabase(CancellationToken cancellationToken)
        {
            await _categoriesDataSeed.SeedCategories(cancellationToken);
        }
    }
}