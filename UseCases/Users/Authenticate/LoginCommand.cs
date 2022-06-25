using System.ComponentModel.DataAnnotations;
using MediatR;

namespace UseCases.Users.Authenticate;

/// <summary>
/// Login command.
/// </summary>
public record LoginCommand : IRequest
{
    /// <summary>
    /// Login.
    /// </summary>
    [Required]
    public string Login { get; init; }

    /// <summary>
    /// Password.
    /// </summary>
    [Required]
    public string Password { get; init; }
}