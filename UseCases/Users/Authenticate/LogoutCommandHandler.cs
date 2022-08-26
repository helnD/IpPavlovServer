using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace UseCases.Users.Authenticate;

/// <summary>
/// Handles <see cref="LogoutCommand"/>.
/// </summary>
public class LogoutCommandHandler : AsyncRequestHandler<LogoutCommand>
{
    private readonly SignInManager<User> signInManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    public LogoutCommandHandler(SignInManager<User> signInManager)
    {
        this.signInManager = signInManager;
    }

    /// <inheritdoc/>
    protected override async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await signInManager.SignOutAsync();
    }
}