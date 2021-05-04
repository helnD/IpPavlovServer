using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace WebApplication
{
    public class Program
    {
        private static IHost _host;

        /// <summary>
        /// Entry point method.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static async Task Main(string[] args)
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
            await _host.RunAsync();
        }

        /// <summary>
        /// Create host builder.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>Host builder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}