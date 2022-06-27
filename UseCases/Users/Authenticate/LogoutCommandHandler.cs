using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace UseCases.Users.Authenticate;

/// <summary>
/// Handles <see cref="LogoutCommand"/>.
/// </summary>
public class LogoutCommandHandler : AsyncRequestHandler<LogoutCommand>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Constructor.
    /// </summary>
    public LogoutCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    protected override async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}