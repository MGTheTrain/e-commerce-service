// <copyright file="Order.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
            this.OrderID = Guid.NewGuid().ToString();
            this.UserID = string.Empty;
            this.OrderDate = DateTime.UtcNow;
            this.TotalAmount = 0.01f;
            this.OrderStatus = "InProgress";
            this.CurrencyCode = "USD";
            this.ReferenceId = Guid.NewGuid().ToString();
            this.AddressLine1 = string.Empty;
            this.AddressLine2 = string.Empty;
            this.AdminArea2 = string.Empty;
            this.AdminArea1 = string.Empty;
            this.PostalCode = string.Empty;
            this.CountryCode = "US";
            this.CheckoutNowHref = "https://www.sandbox.paypal.com/checkoutnow?token=test-order-id";
        }

        [Required]
        public string OrderID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero")]
        public float TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be a 3-letter code")]
        public string CurrencyCode { get; set; }

        public string ReferenceId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AdminArea2 { get; set; }

        public string AdminArea1 { get; set; }

        public string PostalCode { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be a 2-letter code")]
        public string CountryCode { get; set; }

        [Required]
        public string CheckoutNowHref { get; set; }

        /// <summary>
        /// Validates the properties of the Order object.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(this.OrderID))
            {
                yield return new ValidationResult($"{nameof(this.OrderID)} can't be empty");
            }

            if (string.IsNullOrEmpty(this.UserID))
            {
                yield return new ValidationResult($"{nameof(this.UserID)} can't be empty");
            }

            if (this.OrderDate == default(DateTime))
            {
                yield return new ValidationResult($"{nameof(this.OrderDate)} can't be the default value");
            }

            if (this.TotalAmount <= 0)
            {
                yield return new ValidationResult($"{nameof(this.TotalAmount)} must be greater than zero");
            }

            if (string.IsNullOrEmpty(this.OrderStatus))
            {
                yield return new ValidationResult($"{nameof(this.OrderStatus)} can't be empty");
            }

            if (string.IsNullOrEmpty(this.CurrencyCode) || this.CurrencyCode.Length != 3)
            {
                yield return new ValidationResult($"{nameof(this.CurrencyCode)} must be a 3-letter code");
            }

            if (string.IsNullOrEmpty(this.CountryCode) || this.CountryCode.Length != 2)
            {
                yield return new ValidationResult($"{nameof(this.CountryCode)} must be a 2-letter code");
            }

            if (string.IsNullOrEmpty(this.CheckoutNowHref))
            {
                yield return new ValidationResult($"{nameof(this.CheckoutNowHref)} can't be empty");
            }

            yield return ValidationResult.Success;
        }
    }
}
