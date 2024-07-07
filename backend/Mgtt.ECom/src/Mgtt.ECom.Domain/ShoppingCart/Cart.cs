// <copyright file="Cart.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.ShoppingCart
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a shopping cart in the e-commerce system.
    /// </summary>
    public class Cart : IValidatableObject
    {
        public Cart()
        {
            this.CartID = Guid.NewGuid();
            this.UserID = string.Empty;
            this.TotalAmount = 0.0f;
        }

        [Required]
        public Guid CartID { get; internal set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be non-negative")]
        public float TotalAmount { get; set; }

        /// <summary>
        /// Validates the properties of the Cart object.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.CartID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(this.CartID)} can't be empty");
            }

            if (this.UserID == string.Empty)
            {
                yield return new ValidationResult($"{nameof(this.UserID)} can't be empty");
            }

            if (this.TotalAmount < 0)
            {
                yield return new ValidationResult($"{nameof(this.TotalAmount)} can't be negative");
            }

            yield return ValidationResult.Success;
        }
    }
}
