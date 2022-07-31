using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using WebApplication.Commands;

namespace WebApplication;

[Command(Name = "x-ray")]
[Subcommand(typeof(SeedProductsCommand))]
internal sealed class Program
{
    private static IHost _host;

    /// <summary>
    /// Entry point method.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static async Task<int> Main(string[] args)
    {
        // Init host.
        _host = Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            })
            .UseNLog()
            .Build();
        await _host.InitAsync();

        // Command line processing.
        var commandLineApplication = new CommandLineApplication<Program>();
        using var scope = _host.Services.CreateScope();
        commandLineApplication
            .Conventions
            .UseConstructorInjection(scope.ServiceProvider)
            .UseDefaultConventions();
        return await commandLineApplication.ExecuteAsync(args);
    }

    /// <summary>
    /// Command line application execution callback.
    /// </summary>
    /// <returns>Exit code.</returns>
    public async Task<int> OnExecuteAsync()
    {
        await _host.RunAsync();
        return 0;
    }
}