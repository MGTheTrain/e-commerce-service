// <copyright file="OrderItemResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.OrderManagement.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OrderItemResponseDTO
    {
        public Guid OrderItemID { get; set; }

        public string OrderID { get; set; }

        public Guid ProductID { get; set; }

        public string UserID { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }
    }
}
