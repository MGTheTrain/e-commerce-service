// <copyright file="CartItemResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.ShoppingCart.DTOs
{
    using System;

    public class CartItemResponseDTO
    {
        public Guid CartItemID { get; set; }

        public Guid CartID { get; set; }

        public Guid ProductID { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }
    }
}
