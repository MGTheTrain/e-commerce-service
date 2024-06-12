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
        public OrderItem()
        {
            OrderItemID = Guid.NewGuid();
            OrderID = Guid.Empty;
            ProductID = Guid.Empty;
            Quantity = 1;
            Price = 0.01f;
        }

        [Required]
        public Guid OrderItemID { get; internal set; }

        [Required]
        public Guid OrderID { get; set; }

        [Required]
        public Guid ProductID { get; set; }

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
            if (OrderItemID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(OrderItemID)} can't be empty");
            }

            if (OrderID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(OrderID)} can't be empty");
            }

            if (ProductID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(ProductID)} can't be empty");
            }

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
