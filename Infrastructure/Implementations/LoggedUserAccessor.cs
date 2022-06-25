using System;
using System.Linq;
using System.Security.Claims;
using CSharpFunctionalExtensions;
using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Implementations;

/// <summary>
/// Logged user accessor implementation.
/// </summary>
public class LoggedUserAccessor : ILoggedUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Constructor.
    /// </summary>
    public LoggedUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public Result<int> GetCurrentUserId()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            return Result.Failure<int>("There is no active HTTP context specified.");
        }

        var userIdAsString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdAsString))
        {
            return Result.Failure<int>("There is not logged user.");
        }

        var userId = int.Parse(userIdAsString);
        return Result.Success(userId);
    }

    /// <inheritdoc />
    public bool IsAuthenticated()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            return false;
        }

        var userIdAsString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return !string.IsNullOrEmpty(userIdAsString);
    }
}