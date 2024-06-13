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

        /// <summary>
        /// Creates a new shopping cart.
        /// </summary>
        /// <param name="cartDTO">The cart data transfer object containing user ID and total amount.</param>
        /// <returns>A newly created cart.</returns>
        /// <response code="201">Returns the newly created cart.</response>
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

        /// <summary>
        /// Gets a cart by ID.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <returns>The cart with the specified ID.</returns>
        /// <response code="200">Returns the cart with the specified ID.</response>
        /// <response code="404">If the cart is not found.</response>
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

        /// <summary>
        /// Updates an existing cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart to update.</param>
        /// <param name="cartDTO">The cart data transfer object containing the updated total amount.</param>
        /// <response code="204">If the cart was successfully updated.</response>
        /// <response code="404">If the cart is not found.</response>
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

        /// <summary>
        /// Deletes a cart by ID.
        /// </summary>
        /// <param name="cartId">The ID of the cart to delete.</param>
        /// <response code="204">If the cart was successfully deleted.</response>
        /// <response code="404">If the cart is not found.</response>
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

        /// <summary>
        /// Adds an item to a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart to add the item to.</param>
        /// <param name="cartItemDTO">The cart item data transfer object containing product ID, quantity, and price.</param>
        /// <returns>A newly created cart item.</returns>
        /// <response code="201">Returns the newly created cart item.</response>
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

        /// <summary>
        /// Gets all items in a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart to get items from.</param>
        /// <returns>A list of cart items.</returns>
        /// <response code="200">Returns the list of cart items.</response>
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

        /// <summary>
        /// Gets a specific item in a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <param name="itemId">The ID of the cart item.</param>
        /// <returns>The cart item with the specified ID.</returns>
        /// <response code="200">Returns the cart item with the specified ID.</response>
        /// <response code="404">If the cart item is not found.</response>
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

        /// <summary>
        /// Updates an item in a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <param name="itemId">The ID of the cart item to update.</param>
        /// <param name="cartItemDTO">The cart item data transfer object containing updated product ID, quantity, and price.</param>
        /// <response code="204">If the cart item was successfully updated.</response>
        /// <response code="404">If the cart item is not found.</response>
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

        /// <summary>
        /// Deletes an item from a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <param name="itemId">The ID of the cart item to delete.</param>
        /// <response code="204">If the cart item was successfully deleted.</response>
        /// <response code="404">If the cart item is not found.</response>
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