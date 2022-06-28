using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EasyData.EntityFrameworkCore;

namespace Domain;

/// <summary>
/// The manufacturer of the product that cooperates with our company and supplies the goods.
/// </summary>
[MetaEntity(DisplayName = "Партнеры")]
public class Partner
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Partner name.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Название")]
    [Required]
    public string Name { get; init; }

    /// <summary>
    /// Partner description.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Описание")]
    public string Description { get; init; }

    /// <summary>
    /// Products that partner supplies.
    /// </summary>
    public IList<Product> Products { get; init; }

    /// <summary>
    /// Link to site of partner.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Ссылка на сайт")]
    public string Link { get; init; }

    /// <summary>
    /// Image identifier.
    /// </summary>
    public int ImageId { get; set; }

    /// <summary>
    /// Logo of partner.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Логотип")]
    public Image Image { get; set; }
}