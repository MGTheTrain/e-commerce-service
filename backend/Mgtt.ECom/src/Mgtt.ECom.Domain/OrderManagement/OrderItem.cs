using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Domain.OrderManagement
{
    /// <summary>
    /// Represents an order item in the e-commerce system.
    /// </summary>
    public class OrderItem : IValidatableObject
    {
        public int OrderItemID { get; internal set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public float Price { get; set; }

        /// <summary>
        /// Validates the properties of the OrderItem object.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Quantity <= 0)
            {
                yield return new ValidationResult($"{nameof(Quantity)} must be greater than zero");
            }

            if (Price <= 0)
            {
                yield return new ValidationResult($"{nameof(Price)} must be greater than zero");
            }

            yield return ValidationResult.Success;
        }
    }
}
