using System;

namespace Mgtt.ECom.Web.v1.OrderManagement.DTOs
{
    public class OrderResponseDTO
    {
        public Guid OrderID { get; set; }
        public Guid UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalAmount { get; set; }
        public string OrderStatus { get; set; }
    }
}
