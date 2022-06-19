
using EasyData.EntityFrameworkCore;

namespace Domain
{
    /// <summary>
    /// Represents one of current sales leaders.
    /// </summary>
    [MetaEntity(DisplayName = "Лидеры продаж")]
    public record SalesLeader
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Product identifier.
        /// </summary>
        [MetaEntityAttr(DisplayName = "Id продукта")]
        public int ProductId { get; init; }

        /// <summary>
        /// A product that is a sales leader.
        /// </summary>
        [MetaEntityAttr(DisplayName = "Продукт")]
        public Product Product { get; init; }
    }
}