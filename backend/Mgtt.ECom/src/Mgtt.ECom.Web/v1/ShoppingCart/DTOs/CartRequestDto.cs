using System;
using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Web.v1.ShoppingCart.DTOs
{
    public class CartRequestDTO
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be non-negative")]
        public float TotalAmount { get; set; }
    }
}
