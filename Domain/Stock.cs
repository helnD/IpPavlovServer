using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EasyData.EntityFrameworkCore;

namespace Domain
{
    /// <summary>
    /// Products stocks.
    /// </summary>
    [MetaEntity(DisplayName = "Акции")]
    public class Stock
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Save text.
        /// </summary>
        [MetaEntityAttr(DisplayName = "Текст акции")]
        [Required]
        public string Text { get; init; }

        /// <summary>
        /// Related products.
        /// </summary>
        public IEnumerable<Product> RelatedProducts { get; init; }
    }
}