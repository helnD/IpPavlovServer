using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain;

namespace Infrastructure.Abstractions;

/// <summary>
/// Logged user accessor routines.
/// </summary>
public interface ILoggedUserAccessor
{
    /// <summary>
    /// Get current logged user identifier.
    /// </summary>
    /// <returns>Current user identifier.</returns>
    Result<int> GetCurrentUserId();

    /// <summary>
    /// Returns current user;
    /// </summary>
    Task<Result<User>> GetCurrentUserAsync();

    /// <summary>
    /// Return true if there is any user authenticated.
    /// </summary>
    /// <returns>Returns <c>true</c> if there is authenticated user.</returns>
    bool IsAuthenticated();
}