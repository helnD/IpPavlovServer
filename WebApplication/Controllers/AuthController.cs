using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Saritasa.Tools.Domain.Exceptions;
using UseCases.Users.Authenticate;
using WebApplication.ViewModels.Login;

namespace WebApplication.Controllers;

/// <summary>
/// Authentication controller.
/// </summary>
[Route("admin/auth")]
[AllowAnonymous]
public class AuthController : Controller
{
    private readonly IMediator _mediator;
    private readonly SignInManager<User> _signInManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    public AuthController(IMediator mediator, SignInManager<User> signInManager)
    {
        _mediator = mediator;
        _signInManager = signInManager;
    }

    /// <summary>
    /// Index controller.
    /// </summary>
    public IActionResult Index()
    {
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            return RedirectToAction(nameof(EasyDataController.Index), "EasyData");
        }

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