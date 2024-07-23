// <copyright file="OrderResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.OrderManagement.DTOs
{
    using System;

    public class OrderResponseDTO
    {
        public string OrderID { get; set; }

        public string UserID { get; set; }

        public DateTime OrderDate { get; set; }

        public float TotalAmount { get; set; }

        public string OrderStatus { get; set; }

        public string CurrencyCode { get; set; }

        public string ReferenceId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AdminArea2 { get; set; }

        public string AdminArea1 { get; set; }

        public string PostalCode { get; set; }

        public string CountryCode { get; set; }
    }
}
