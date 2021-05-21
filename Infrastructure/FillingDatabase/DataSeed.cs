using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.FillingDatabase
{
    /// <summary>
    /// Class for database initial seed.
    /// </summary>
    public class DataSeed
    {
        private readonly CategoriesDataSeed _categoriesDataSeed;
        private readonly PartnersDataSeed _partnersDataSeed;
        private readonly ProductsDataSeed _productsDataSeed;
        private readonly CertificatesDataSeed _certificatesDataSeed;

        public DataSeed(CategoriesDataSeed categoriesDataSeed, PartnersDataSeed partnersDataSeed,
            ProductsDataSeed productsDataSeed, CertificatesDataSeed certificatesDataSeed)
        {
            _categoriesDataSeed = categoriesDataSeed;
            _partnersDataSeed = partnersDataSeed;
            _productsDataSeed = productsDataSeed;
            _certificatesDataSeed = certificatesDataSeed;
        }

        /// <summary>
        /// Seed categories.
        /// </summary>
        public async Task SeedTestDatabase(CancellationToken cancellationToken)
        {
            await _certificatesDataSeed.SeedCategories(cancellationToken);
            await _categoriesDataSeed.SeedCategories(cancellationToken);
            await _partnersDataSeed.SeedPartners(cancellationToken);
            await _productsDataSeed.SeedProducts(cancellationToken);
        }
    }
}