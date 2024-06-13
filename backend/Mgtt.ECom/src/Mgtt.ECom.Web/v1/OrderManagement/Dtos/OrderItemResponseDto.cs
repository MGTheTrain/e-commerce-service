using System;
using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Web.v1.OrderManagement.DTOs
{
    public class OrderItemResponseDTO
    {
        public Guid OrderItemID { get; set; }
        public Guid OrderID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
    }
}
