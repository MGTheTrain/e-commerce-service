// <copyright file="Order.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.OrderManagement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents an order in the e-commerce system.
    /// </summary>
    public class Order : IValidatableObject
    {
        public Order()
        {
            this.OrderID = Guid.NewGuid();
            this.UserID = Guid.Empty;
            this.OrderDate = DateTime.UtcNow;
            this.TotalAmount = 0.01f;
            this.OrderStatus = "InProgress";
        }

        [Required]
        public Guid OrderID { get; internal set; }

        [Required]
        public Guid UserID { get; set; }

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
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.OrderID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(this.OrderID)} can't be empty");
            }

            if (this.UserID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(this.UserID)} can't be empty");
            }

            if (this.OrderDate == default(DateTime))
            {
                yield return new ValidationResult($"{nameof(this.OrderDate)} can't be default value");
            }

            if (this.TotalAmount <= 0)
            {
                yield return new ValidationResult($"{nameof(this.TotalAmount)} must be greater than zero");
            }

            if (string.IsNullOrEmpty(this.OrderStatus))
            {
                yield return new ValidationResult($"{nameof(this.OrderStatus)} can't be empty");
            }

            yield return ValidationResult.Success;
        }
    }
}
