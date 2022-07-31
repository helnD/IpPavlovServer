using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyData.EntityFrameworkCore;

namespace Domain;

/// <summary>
/// Question from site.
/// </summary>
[MetaEntity(DisplayName = "Вопросы")]
public record Question
{
    /// <summary>
    /// Question id.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Name of user who ask a question.
    /// </summary>
    [Required]
    [MetaEntityAttr(DisplayName = "Имя отправителя")]
    public string Owner { get; init; }

    /// <summary>
    /// Email of owner for feedback.
    /// </summary>
    [Required]
    [MetaEntityAttr(DisplayName = "Email отправителя")]
    public string OwnerEmail { get; init; }

    /// <summary>
    /// Question text.
    /// </summary>
    [Required]
    [MetaEntityAttr(DisplayName = "Текст сообщения")]
    public string QuestionText { get; init; }

    /// <summary>
    /// Date and time when email was send.
    /// </summary>
    [Column(TypeName = "timestamptz")]
    [Required]
    [MetaEntityAttr(DisplayName = "Дата отправления")]
    public DateTime SendingTime { get; init; } = DateTime.Now;
}