using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Controllers;
using WebApplication.Infrastructure.Attributes;

/// <summary>
/// Controller for add/edit content.
/// </summary>
[Route("easydata")]
[WebUiAuthorize]
public class EasyDataController : Controller
{
    private readonly ILogger<EasyDataController> _logger;
    private readonly ILoggedUserAccessor _loggedUserAccessor;

    /// <summary>
    /// Constructor.
    /// </summary>
    public EasyDataController(ILogger<EasyDataController> logger,
        ILoggedUserAccessor loggedUserAccessor)
    {
        _logger = logger;
        _loggedUserAccessor = loggedUserAccessor;
    }

    /// <summary>
    /// Index controller.
    /// </summary>
    [Route("{**entity}")]
    public IActionResult Index(string entity)
    {
        if (!_loggedUserAccessor.IsAuthenticated())
        {
            RedirectToAction(nameof(AuthController.Index), "Auth");
        }
        return View();
    }
}