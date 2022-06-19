﻿using System;

namespace Domain
{
    /// <summary>
    /// Product that we supply.
    /// </summary>
    public record Product
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Number of product position in excel document.
        /// </summary>
        public int DocumentId { get; init; }

        /// <summary>
        /// Product code.
        /// </summary>
        public int Code { get; init; }

        /// <summary>
        /// Product description.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Quantity of products.
        /// </summary>
        public int Quantity { get; init; }

        /// <summary>
        /// Unit of measurement.
        /// </summary>
        public string Unit { get; init; }

        /// <summary>
        /// Price of product.
        /// </summary>
        public decimal Price { get; init; }

        public int CategoryId { get; init; }

        /// <summary>
        /// Category of product.
        /// </summary>
        public Category Category { get; init; }

        /// <summary>
        /// Partner that supply product.
        /// </summary>
        public Partner Producer { get; init; }

        /// <summary>
        /// Image of product.
        /// </summary>
        public Image Image { get; init; }
    }
}