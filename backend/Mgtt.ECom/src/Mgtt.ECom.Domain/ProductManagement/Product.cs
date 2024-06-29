// <copyright file="Product.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.ProductManagement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a product in the e-commerce system.
    /// </summary>
    public class Product : IValidatableObject
    {
        public Product()
        {
            this.ProductID = Guid.NewGuid();
            this.CategoryID = Guid.Empty;
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Price = 0.01f;
            this.Stock = 0;
            this.ImageUrl = string.Empty;
        }

        [Required]
        public Guid ProductID { get; internal set; }

        [Required]
        public Guid CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public float Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be non-negative")]
        public int Stock { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Validates the properties of the Product object.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ProductID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(this.ProductID)} can't be empty");
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                yield return new ValidationResult($"{nameof(this.Name)} can't be empty");
            }

            if (this.Price <= 0)
            {
                yield return new ValidationResult($"{nameof(this.Price)} must be greater than zero");
            }

            if (this.Stock < 0)
            {
                yield return new ValidationResult($"{nameof(this.Stock)} can't be negative");
            }

            yield return ValidationResult.Success;
        }
    }
}
