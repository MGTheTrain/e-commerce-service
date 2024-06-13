using System;

namespace Mgtt.ECom.Web.v1.ShoppingCart.DTOs
{
    public class CartResponseDTO
    {
        public Guid CartID { get; set; }
        public Guid UserID { get; set; }
        public float TotalAmount { get; set; }
    }
}
