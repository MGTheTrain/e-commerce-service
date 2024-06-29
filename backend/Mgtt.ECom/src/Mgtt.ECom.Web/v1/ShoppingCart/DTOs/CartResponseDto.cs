// <copyright file="CartResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.ShoppingCart.DTOs
{
    using System;

    public class CartResponseDTO
    {
        public Guid CartID { get; set; }

        public Guid UserID { get; set; }

        public float TotalAmount { get; set; }
    }
}
