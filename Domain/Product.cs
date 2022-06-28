using System;
using System.ComponentModel.DataAnnotations;
using EasyData.EntityFrameworkCore;

namespace Domain;

/// <summary>
/// Product that we supply.
/// </summary>
[MetaEntity(DisplayName = "Продукты")]
public record Product
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Number of product position in excel document.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Id в документе")]
    [Required]
    public int DocumentId { get; init; }

    /// <summary>
    /// Product code.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Код продукта")]
    [Required]
    public int Code { get; init; }

    /// <summary>
    /// Product description.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Описание")]
    public string Description { get; init; }

    /// <summary>
    /// Quantity of products.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Количество")]
    [Required]
    public int Quantity { get; init; }

    /// <summary>
    /// Unit of measurement.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Единицы")]
    [Required]
    public string Unit { get; init; }

    /// <summary>
    /// Price of product.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Цена")]
    [Required]
    public decimal Price { get; init; }

    /// <summary>
    /// Category identifier.
    /// </summary>
    [MetaEntityAttr(ShowOnView = false)]
    public int CategoryId { get; init; }

    /// <summary>
    /// Category of product.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Категория")]
    public Category Category { get; init; }

    /// <summary>
    /// Producer identifier.
    /// </summary>
    [MetaEntityAttr(ShowOnView = false)]
    public int ProducerId { get; init; }

    /// <summary>
    /// Partner that supply product.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Производитель")]
    public Partner Producer { get; init; }

    /// <summary>
    /// Image identifier.
    /// </summary>
    [MetaEntityAttr(ShowOnView = false)]
    public int ImageId { get; init; }

    /// <summary>
    /// Image of product.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Изображение")]
    public Image Image { get; init; }

    /// <summary>
    /// Related stock.
    /// </summary>
    [MetaEntityAttr(ShowOnView = false)]
    public int? StockId { get; set; }

    /// <summary>
    /// Stock.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Участвует в акции")]
    public Stock Stock { get; set; }
}