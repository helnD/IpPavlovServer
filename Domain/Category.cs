using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EasyData.EntityFrameworkCore;

namespace Domain;

/// <summary>
/// Category of product.
/// </summary>
[MetaEntity(DisplayName = "Категории")]
public record Category
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Category name.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Название")]
    [Required]
    public string Name { get; init; }

    /// <summary>
    /// Name for routing transition.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Путь к категории")]
    [Required]
    public string RouteName { get; init; }

    /// <summary>
    /// Products list.
    /// </summary>
    public IList<Product> Products { get; init; }

    /// <summary>
    /// Icon identifier.
    /// </summary>
    [MetaEntityAttr(ShowOnView = false)]
    public int IconId { get; set; }

    /// <summary>
    /// Icon of category.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Иконка")]
    public Image Icon { get; set; }
}