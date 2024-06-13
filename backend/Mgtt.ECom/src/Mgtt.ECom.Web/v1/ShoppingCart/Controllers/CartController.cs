using Microsoft.AspNetCore.Mvc;
using Mgtt.ECom.Application.DTOs.ShoppingCart;
using Mgtt.ECom.Domain.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mgtt.ECom.Web.v1.ShoppingCart.Controllers
{
    [Route("api/v1/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;

        public CartController(ICartService cartService, ICartItemService cartItemService)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart(CartRequestDTO cartDTO)
        {
            var cart = new Cart
            {
                UserID = cartDTO.UserID,
                TotalAmount = cartDTO.TotalAmount
            };

            await _cartService.CreateCart(cart);

            return CreatedAtAction(nameof(GetCartById), new { cartId = cart.CartID }, new CartResponseDTO
            {
                CartID = cart.CartID,
                UserID = cart.UserID,
                TotalAmount = cart.TotalAmount
            });
        }

        [HttpGet("{cartId}")]
        public async Task<ActionResult<CartResponseDTO>> GetCartById(Guid cartId)
        {
            var cart = await _cartService.GetCartByUserId(cartId);

            if (cart == null)
            {
                return NotFound();
            }

            return Ok(new CartResponseDTO
            {
                CartID = cart.CartID,
                UserID = cart.UserID,
                TotalAmount = cart.TotalAmount
            });
        }

        [HttpPut("{cartId}")]
        public async Task<IActionResult> UpdateCart(Guid cartId, CartRequestDTO cartDTO)
        {
            var cart = await _cartService.GetCartByUserId(cartId);

            if (cart == null)
            {
                return NotFound();
            }

            cart.TotalAmount = cartDTO.TotalAmount;

            await _cartService.UpdateCart(cart);

            return NoContent();
        }

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCart(Guid cartId)
        {
            var cart = await _cartService.GetCartByUserId(cartId);

            if (cart == null)
            {
                return NotFound();
            }

            await _cartService.DeleteCart(cartId);

            return NoContent();
        }

        [HttpPost("{cartId}/items")]
        public async Task<IActionResult> CreateCartItem(Guid cartId, CartItemRequestDTO cartItemDTO)
        {
            var cartItem = new CartItem
            {
                CartID = cartId,
                ProductID = cartItemDTO.ProductID,
                Quantity = cartItemDTO.Quantity,
                Price = cartItemDTO.Price
            };

            await _cartItemService.CreateCartItem(cartItem);

            return CreatedAtAction(nameof(GetCartItemById), new { cartId, itemId = cartItem.CartItemID }, new CartItemResponseDTO
            {
                CartItemID = cartItem.CartItemID,
                CartID = cartItem.CartID,
                ProductID = cartItem.ProductID,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            });
        }

        [HttpGet("{cartId}/items")]
        public async Task<ActionResult<IEnumerable<CartItemResponseDTO>>> GetCartItemsByCartId(Guid cartId)
        {
            var cartItems = await _cartItemService.GetCartItemsByCartId(cartId);
            var cartItemDTOs = new List<CartItemResponseDTO>();

            foreach (var cartItem in cartItems)
            {
                cartItemDTOs.Add(new CartItemResponseDTO
                {
                    CartItemID = cartItem.CartItemID,
                    CartID = cartItem.CartID,
                    ProductID = cartItem.ProductID,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price
                });
            }

            return Ok(cartItemDTOs);
        }

        [HttpGet("{cartId}/items/{itemId}")]
        public async Task<ActionResult<CartItemResponseDTO>> GetCartItemById(Guid cartId, Guid itemId)
        {
            var cartItem = await _cartItemService.GetCartItemById(itemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            return Ok(new CartItemResponseDTO
            {
                CartItemID = cartItem.CartItemID,
                CartID = cartItem.CartID,
                ProductID = cartItem.ProductID,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            });
        }

        [HttpPut("{cartId}/items/{itemId}")]
        public async Task<IActionResult> UpdateCartItem(Guid cartId, Guid itemId, CartItemRequestDTO cartItemDTO)
        {
            var cartItem = await _cartItemService.GetCartItemById(itemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.CartID = cartItemDTO.CartID;
            cartItem.ProductID = cartItemDTO.ProductID;
            cartItem.Quantity = cartItemDTO.Quantity;
            cartItem.Price = cartItemDTO.Price;

            await _cartItemService.UpdateCartItem(cartItem);

            return NoContent();
        }

        [HttpDelete("{cartId}/items/{itemId}")]
        public async Task<IActionResult> DeleteCartItem(Guid cartId, Guid itemId)
        {
            var cartItem = await _cartItemService.GetCartItemById(itemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            await _cartItemService.DeleteCartItem(itemId);

            return NoContent();
        }
    }
}
