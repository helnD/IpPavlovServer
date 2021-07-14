using System.Collections.Generic;

namespace Domain
{
    /// <summary>
    /// The manufacturer of the product that cooperates with our company and supplies the goods.
    /// </summary>
    public class Partner
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Partner name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Partner description.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Products that partner supplies.
        /// </summary>
        public IList<Product> Products { get; init; }

        /// <summary>
        /// Link to site of partner.
        /// </summary>
        public string Link { get; init; }

        /// <summary>
        /// Logo of partner.
        /// </summary>
        public Image Image { get; set; }
    }
}