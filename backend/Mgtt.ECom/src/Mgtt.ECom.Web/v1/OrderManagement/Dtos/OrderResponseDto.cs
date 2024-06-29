// <copyright file="OrderResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.OrderManagement.DTOs
{
    using System;

    public class OrderResponseDTO
    {
        public Guid OrderID { get; set; }

        public Guid UserID { get; set; }

        public DateTime OrderDate { get; set; }

        public float TotalAmount { get; set; }

        public string OrderStatus { get; set; }
    }
}
