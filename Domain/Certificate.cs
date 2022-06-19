using EasyData.EntityFrameworkCore;

namespace Domain
{
    /// <summary>
    /// Certificate.
    /// </summary>
    [MetaEntity(DisplayName = "Сертификаты")]
    public record Certificate
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Certificate description.
        /// </summary>
        [MetaEntityAttr(DisplayName = "Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Image of certificate.
        /// </summary>
        [MetaEntityAttr(DisplayName = "Изображение")]
        public Image Image { get; set; }

        /// <summary>
        /// Image id.
        /// </summary>
        [MetaEntityAttr(ShowOnView = false)]
        public int ImageId { get; init; }
    }
}