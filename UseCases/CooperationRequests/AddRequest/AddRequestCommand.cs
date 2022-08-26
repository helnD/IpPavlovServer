using System.ComponentModel.DataAnnotations;
using MediatR;

namespace UseCases.CooperationRequests.AddRequest;

/// <summary>
/// Command to cooperation request adding.
/// </summary>
public record AddRequestCommand : IRequest
{
    /// <summary>
    /// Company name.
    /// </summary>
    [Required]
    public string Company { get; init; }

    /// <summary>
    /// Representative name.
    /// </summary>
    [Required]
    public string Name { get; init; }

    /// <summary>
    /// Contact email.
    /// </summary>
    [Required]
    public string Email { get; init; }

    /// <summary>
    /// Phone number of representative.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Phone { get; set; }
}