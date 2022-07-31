using System.ComponentModel.DataAnnotations;
using EasyData.EntityFrameworkCore;

namespace Domain;

/// <summary>
/// Local stored image.
/// </summary>
[MetaEntity(DisplayName = "Изображения")]
public record Image
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Name and extension of file.
    /// </summary>
    public string Name => System.IO.Path.GetFileName(Path);

    /// <summary>
    /// Path to image.
    /// </summary>
    [MetaEntityAttr(ShowOnCreate = false, ShowOnEdit = false)]
    [Required]
    public string Path { get; init; }
}