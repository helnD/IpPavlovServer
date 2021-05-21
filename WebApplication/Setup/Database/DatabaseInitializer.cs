using System.Threading.Tasks;
using Extensions.Hosting.AsyncInitialization;
using Infrastructure;
using Infrastructure.DataAccess;
using Infrastructure.FillingDatabase;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Setup.Database
{
    /// <summary>
    /// Database initializer.
    /// </summary>
    public class DatabaseInitializer : IAsyncInitializer
    {
        private readonly AppDbContext _context;
        private readonly DataSeed _dataSeed;

        public DatabaseInitializer(AppDbContext context, DataSeed dataSeed)
        {
            _context = context;
            _dataSeed = dataSeed;
        }

        /// <summary>
        /// Database initialization.
        /// </summary>
        public async Task InitializeAsync()
        {
            await _context.Database.MigrateAsync();
            await _dataSeed.SeedTestDatabase(default);
        }
    }
}