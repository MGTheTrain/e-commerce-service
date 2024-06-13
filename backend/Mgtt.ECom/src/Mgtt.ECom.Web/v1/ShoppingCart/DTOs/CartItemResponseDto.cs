using System;

namespace Mgtt.ECom.Web.v1.ShoppingCart.DTOs
{
    public class CartItemResponseDTO
    {
        public Guid CartItemID { get; set; }
        public Guid CartID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
    }
}
