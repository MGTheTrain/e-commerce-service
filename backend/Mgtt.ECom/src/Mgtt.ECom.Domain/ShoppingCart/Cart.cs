using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Domain.ShoppingCart
{
    /// <summary>
    /// Represents a shopping cart in the e-commerce system.
    /// </summary>
    public class Cart : IValidatableObject
    {
        public int CartID { get; internal set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be non-negative")]
        public float TotalAmount { get; set; }

        /// <summary>
        /// Validates the properties of the Cart object.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TotalAmount < 0)
            {
                yield return new ValidationResult($"{nameof(TotalAmount)} can't be negative");
            }

            yield return ValidationResult.Success;
        }
    }
}
