using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[Route("easydata")]
public class EasyDataController : Controller
{
    private readonly ILogger<EasyDataController> _logger;

    public EasyDataController(ILogger<EasyDataController> logger)
    {
        _logger = logger;
    }

    [Route("{**entity}")]
    public IActionResult Index(string entity)
    {
        if (string.IsNullOrEmpty(entity))
        {
            _logger.LogInformation("Index page");
        }
        else
        {
            _logger.LogInformation($"{entity} page");
        }

        return View();
    }
}