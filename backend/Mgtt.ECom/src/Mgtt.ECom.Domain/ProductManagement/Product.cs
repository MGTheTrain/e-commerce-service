using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Domain.ProductManagement
{
    /// <summary>
    /// Represents a product in the e-commerce system.
    /// </summary>
    public class Product : IValidatableObject
    {
        public Product()
        {
            ProductID = Guid.NewGuid();
            CategoryID = Guid.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Price = 0.01f;
            Stock = 0;
            ImageUrl = string.Empty;
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProductID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(ProductID)} can't be empty");
            }

            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult($"{nameof(Name)} can't be empty");
            }

            if (Price <= 0)
            {
                yield return new ValidationResult($"{nameof(Price)} must be greater than zero");
            }

            if (Stock < 0)
            {
                yield return new ValidationResult($"{nameof(Stock)} can't be negative");
            }

            yield return ValidationResult.Success;
        }
    }
}
