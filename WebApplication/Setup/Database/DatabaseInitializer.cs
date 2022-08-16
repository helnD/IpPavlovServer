using System.Threading.Tasks;
using Domain;
using Extensions.Hosting.AsyncInitialization;
using Infrastructure;
using Infrastructure.DataAccess;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication.FillingDatabase;

namespace WebApplication.Setup.Database;

/// <summary>
/// Database initializer.
/// </summary>
public class DatabaseInitializer : IAsyncInitializer
{
    private readonly AppDbContext _context;
    private readonly DataSeed _dataSeed;
    private readonly UserManager<User> _userManager;
    private readonly AdminSettings adminSettings;

    public DatabaseInitializer(AppDbContext context,
        DataSeed dataSeed,
        UserManager<User> userManager,
        IOptions<AdminSettings> adminSettings)
    {
        _context = context;
        _dataSeed = dataSeed;
        _userManager = userManager;
        this.adminSettings = adminSettings.Value;
    }

    /// <summary>
    /// Database initialization.
    /// </summary>
    public async Task InitializeAsync()
    {
        await _context.Database.MigrateAsync();
        await _dataSeed.SeedInitialDatabase(default);
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
            Email = adminSettings.Username,
            UserName = adminSettings.Username,
            FirstName = adminSettings.FirstName,
            LastName = adminSettings.LastName,
            PhoneNumber = adminSettings.Phone
        };

        var result = await _userManager.CreateAsync(admin, adminSettings.Password);
        if (result.Succeeded)
        {
            await _context.SaveChangesAsync(default);
        }
    }
}