using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using WebApplication.FillingDatabase;

namespace WebApplication.Commands;

/// <summary>
/// Seed products and corresponding images using zip archive.
/// </summary>
[Command("seed-products")]
public class SeedProductsCommand
{
    private readonly ProductsDataSeed _productsDataSeed;

    /// <summary>
    /// Constructor.
    /// </summary>
    public SeedProductsCommand(ProductsDataSeed productsDataSeed)
    {
        _productsDataSeed = productsDataSeed;
    }

    /// <summary>
    /// Path to archive.
    /// </summary>
    [Option( "--path-to-archive")]
    public string PathToArchive { get; }

    /// <summary>
    /// Command line application execution callback.
    /// </summary>
    public async Task OnExecuteAsync(CancellationToken cancellationToken)
    {
        await _productsDataSeed.SeedProducts(PathToArchive, cancellationToken);
    }
}