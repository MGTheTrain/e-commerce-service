// <copyright file="CartRequestDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.ShoppingCart.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CartRequestDTO
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be non-negative")]
        public float TotalAmount { get; set; }
    }
}
