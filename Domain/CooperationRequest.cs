using System.ComponentModel.DataAnnotations;
using EasyData.EntityFrameworkCore;

namespace Domain;

/// <summary>
/// Cooperation requests from different companies.
/// </summary>
[MetaEntity(DisplayName = "Запросы на сотрудничество")]
public class CooperationRequest
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Company name.
    /// </summary>
    [Required]
    [MetaEntityAttr(DisplayName = "Название компании")]
    public string Company { get; init; }

    /// <summary>
    /// Representative name.
    /// </summary>
    [Required]
    [MetaEntityAttr(DisplayName = "Представитель")]
    public string Name { get; init; }

    /// <summary>
    /// Contact email.
    /// </summary>
    [Required]
    [MetaEntityAttr(DisplayName = "E-Mail")]
    public string Email { get; init; }
}