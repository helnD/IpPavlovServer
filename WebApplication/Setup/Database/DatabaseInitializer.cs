using System.Threading.Tasks;
using Domain;
using Extensions.Hosting.AsyncInitialization;
using Infrastructure;
using Infrastructure.DataAccess;
using Infrastructure.FillingDatabase;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;

        public DatabaseInitializer(AppDbContext context, DataSeed dataSeed, UserManager<User> userManager)
        {
            _context = context;
            _dataSeed = dataSeed;
            _userManager = userManager;
        }

        /// <summary>
        /// Database initialization.
        /// </summary>
        public async Task InitializeAsync()
        {
            await _context.Database.MigrateAsync();
            await _dataSeed.SeedTestDatabase(default);
            await AddAdminUser();
        }

        private async Task AddAdminUser()
        {
            var thereIsNoUser = !await _context.Users.AnyAsync();
            if (!thereIsNoUser)
            {
                return;
            }

            var admin = new User
            {
                Email = "admin@example.com",
                UserName = "admin@example.com",
                FirstName = "Admin",
                LastName = "Admin",
                PhoneNumber = "1111111111"
            };

            var result = await _userManager.CreateAsync(admin, "Qwert123#");
            if (result.Succeeded)
            {
                await _context.SaveChangesAsync(default);
            }
        }
    }
}