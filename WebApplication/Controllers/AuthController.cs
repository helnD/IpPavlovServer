using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Saritasa.Tools.Domain.Exceptions;
using UseCases.Users.Authenticate;
using WebApplication.ViewModels.Login;

namespace WebApplication.Controllers;

/// <summary>
/// Authentication controller.
/// </summary>
[Route("auth")]
public class AuthController : Controller
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor.
    /// </summary>
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Index controller.
    /// </summary>
    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    /// <summary>
    /// Login.
    /// </summary>
    /// <param name="viewModel">View model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), viewModel);
        }

        try
        {
            var command = new LoginCommand
            {
                Login = viewModel.Login,
                Password = viewModel.Password,
            };
            await _mediator.Send(command, cancellationToken);
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(nameof(Index), viewModel);
        }

        return RedirectToAction(nameof(EasyDataController.Index), "EasyData");
    }

    /// <summary>
    /// Log out user from the application.
    /// </summary>
    [Route("[action]")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await _mediator.Send(new LogoutCommand(), cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}