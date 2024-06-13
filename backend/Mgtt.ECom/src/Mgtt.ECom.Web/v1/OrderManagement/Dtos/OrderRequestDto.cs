using System;
using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Web.v1.OrderManagement.DTOs
{
    public class OrderRequestDTO
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero")]
        public float TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; }
    }
}
