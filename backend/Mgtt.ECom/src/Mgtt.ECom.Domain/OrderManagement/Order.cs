using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Domain.OrderManagement
{
    /// <summary>
    /// Represents an order in the e-commerce system.
    /// </summary>
    public class Order : IValidatableObject
    {
        public int OrderID { get; internal set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero")]
        public float TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; }

        /// <summary>
        /// Validates the properties of the Order object.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OrderDate == default(DateTime))
            {
                yield return new ValidationResult($"{nameof(OrderDate)} can't be default value");
            }

            if (TotalAmount <= 0)
            {
                yield return new ValidationResult($"{nameof(TotalAmount)} must be greater than zero");
            }

            if (string.IsNullOrEmpty(OrderStatus))
            {
                yield return new ValidationResult($"{nameof(OrderStatus)} can't be empty");
            }

            yield return ValidationResult.Success;
        }
    }
}
