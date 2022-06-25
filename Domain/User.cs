using System.ComponentModel.DataAnnotations;
using EasyData.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Domain;

/// <summary>
/// User.
/// </summary>
[MetaEntity(DisplayName = "Пользователи")]
public class User : IdentityUser<int>
{
    /// <summary>
    /// First name.
    /// </summary>
    [Required]
    [MaxLength(80)]
    public string FirstName { get; set; }

    /// <summary>
    /// Last name.
    /// </summary>
    [Required]
    [MaxLength(80)]
    public string LastName { get; set; }

    /// <summary>
    /// Email.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public override string Email { get; set; }

    /// <summary>
    /// Phone number.
    /// </summary>
    [MaxLength(20)]
    public override  string PhoneNumber { get; set; }
}