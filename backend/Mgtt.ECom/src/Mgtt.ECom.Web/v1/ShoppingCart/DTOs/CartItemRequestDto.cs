// <copyright file="CartItemRequestDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.ShoppingCart.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CartItemRequestDTO
    {
        [Required]
        public Guid CartID { get; set; }

        [Required]
        public Guid ProductID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public float Price { get; set; }
    }
}
