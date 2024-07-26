// <copyright file="OrderRequestDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.OrderManagement.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OrderRequestDTO
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero")]
        public float TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; }

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
    }
}
