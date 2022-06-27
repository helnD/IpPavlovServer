using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain;
using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations;

/// <summary>
/// Logged user accessor implementation.
/// </summary>
public class LoggedUserAccessor : ILoggedUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDbContext _dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    public LoggedUserAccessor(IHttpContextAccessor httpContextAccessor,
        IDbContext dbContext)
    {
        this._httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
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
    public async Task<Result<User>> GetCurrentUserAsync()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            return Result.Failure<User>("There is no active HTTP context specified.");
        }

        var userIdAsString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdAsString))
        {
            return Result.Failure<User>("There is not logged user.");
        }

        var userId = int.Parse(userIdAsString);
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
        return Result.Success(user);
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