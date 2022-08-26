using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Saritasa.Tools.Domain.Exceptions;

namespace UseCases.Users.Authenticate;

/// <summary>
/// Handles <see cref="LoginCommand"/>.
/// </summary>
internal class LoginCommandHandler : AsyncRequestHandler<LoginCommand>
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Constructor.
    /// </summary>
    public LoginCommandHandler(SignInManager<User> signInManager,
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    protected override async Task Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Login, request.Password,
            lockoutOnFailure: false, isPersistent: true);
        if (!result.Succeeded)
        {
            throw new ForbiddenException("Неверный логин или пароль");
        }
    }
}