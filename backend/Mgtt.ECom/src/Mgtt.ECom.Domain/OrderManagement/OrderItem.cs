// <copyright file="OrderItem.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.OrderManagement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents an order item in the e-commerce system.
    /// </summary>
    public class OrderItem : IValidatableObject
    {
        public OrderItem()
        {
            this.OrderItemID = Guid.NewGuid();
            this.OrderID = Guid.Empty;
            this.ProductID = Guid.Empty;
            this.Quantity = 1;
            this.Price = 0.01f;
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
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.OrderItemID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(this.OrderItemID)} can't be empty");
            }

            if (this.OrderID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(this.OrderID)} can't be empty");
            }

            if (this.ProductID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(this.ProductID)} can't be empty");
            }

            if (this.Quantity <= 0)
            {
                yield return new ValidationResult($"{nameof(this.Quantity)} must be greater than zero");
            }

            if (this.Price <= 0)
            {
                yield return new ValidationResult($"{nameof(this.Price)} must be greater than zero");
            }

            yield return ValidationResult.Success;
        }
    }
}
