using System;
using System.ComponentModel.DataAnnotations;
using EasyData.EntityFrameworkCore;
using DataType = EasyData.DataType;

namespace Domain;

/// <summary>
/// Our sales representative.
/// </summary>
[MetaEntity(DisplayName = "Торговые представители")]
public record SalesRepresentative
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Region where sales representative works.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Район")]
    [Required]
    public string Region { get; init; }

    /// <summary>
    /// Representative firstname.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Имя представителя")]
    [Required]
    public string FirstName { get; init; }

    /// <summary>
    /// Representative lastname.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Фамилия представителя")]
    [Required]
    public string LastName { get; init; }

    /// <summary>
    /// Middle name.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Отчество представителя")]
    [Required]
    public string MiddleName { get; init; }

    /// <summary>
    /// Phone number.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Телефон")]
    [Required]
    public string Phone { get; init; }

    /// <summary>
    /// Time when representative begins work.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Начало рабочего дня", DataType = DataType.Time)]
    [Required]
    public TimeSpan StartOfWork { get; init; }

    /// <summary>
    /// Time when representative ends work.
    /// </summary>
    [MetaEntityAttr(DisplayName = "Конец рабочего дня", DataType = DataType.Time)]
    [Required]
    public TimeSpan EndOfWork { get; init; }
}