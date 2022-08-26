using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Controllers;
using WebApplication.Infrastructure.Attributes;

/// <summary>
/// Controller for add/edit content.
/// </summary>
[Route("admin/easydata")]
public class EasyDataController : Controller
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public EasyDataController()
    {
    }

    /// <summary>
    /// Index controller.
    /// </summary>
    [Route("{**entity}")]
    public IActionResult Index(string entity)
    {
        return View();
    }
}