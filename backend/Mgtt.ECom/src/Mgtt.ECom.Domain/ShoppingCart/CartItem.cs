using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Domain.ShoppingCart
{
    /// <summary>
    /// Represents a shopping cart item in the e-commerce system.
    /// </summary>
    public class CartItem : IValidatableObject
    {
        public CartItem()
        {
            CartItemID = Guid.NewGuid();
            CartID = Guid.Empty;
            ProductID = Guid.Empty;
            Quantity = 1;
            Price = 0.01f;
        }

        [Required]
        public Guid CartItemID { get; internal set; }

        [Required]
        public Guid CartID { get; set; }

        [Required]
        public Guid ProductID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public float Price { get; set; }

        /// <summary>
        /// Validates the properties of the CartItem object.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CartItemID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(CartID)} can't be empty");
            }

            if (CartID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(CartID)} can't be empty");
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
