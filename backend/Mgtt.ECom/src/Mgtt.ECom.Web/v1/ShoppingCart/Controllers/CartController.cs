using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mgtt.ECom.Domain.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mgtt.ECom.Web.v1.ShoppingCart.DTOs;
using static System.Collections.Specialized.BitVector32;

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
        /// <response code="400">If the cart data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CartResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCart(CartRequestDTO cartDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cart = new Cart
            {
                UserID = cartDTO.UserID,
                TotalAmount = cartDTO.TotalAmount
            };

            var action = await _cartService.CreateCart(cart);
            if (action == null)
            {
                return BadRequest();
            }

            var cartResponseDTO = new CartResponseDTO
            {
                CartID = cart.CartID,
                UserID = cart.UserID,
                TotalAmount = cart.TotalAmount
            };

            return CreatedAtAction(nameof(CreateCart), cartResponseDTO);
        }

        /// <summary>
        /// Gets a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <returns>The cart with the specified ID.</returns>
        /// <response code="200">Returns the cart with the specified ID.</response>
        /// <response code="404">If the cart is not found.</response>
        [HttpGet("{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartResponseDTO>> GetCartById(Guid cartId)
        {
            var cart = await _cartService.GetCartById(cartId);

            if (cart == null)
            {
                return NotFound();
            }

            var cartResponseDto = new CartResponseDTO // utilize auto-mapper for optimization
            {
                CartID = cart.CartID,
                UserID = cart.UserID,
                TotalAmount = cart.TotalAmount
            };

            return Ok(cartResponseDto);
        }

        /// <summary>
        /// Retrieves all carts.
        /// </summary>
        /// <returns>A list of all carts.</returns>
        /// <response code="200">Returns a list of all carts.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CartResponseDTO>))]
        public async Task<ActionResult<IEnumerable<CartResponseDTO>>> GetAllCarts()
        {
            var carts = await _cartService.GetAllCarts();
            var cartDTOs = new List<CartResponseDTO>();

            foreach (var cart in carts)
            {
                cartDTOs.Add(new CartResponseDTO
                {
                    CartID = cart.CartID,
                    UserID = cart.UserID,
                    TotalAmount = cart.TotalAmount
                });
            }

            return Ok(cartDTOs);
        }


        /// <summary>
        /// Updates an existing cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart user.</param>
        /// <param name="cartDTO">The cart data transfer object containing the updated total amount.</param>
        /// <response code="204">If the cart was successfully updated.</response>
        /// <response code="404">If the cart is not found.</response>
        /// <response code="400">If the cart data is invalid.</response>
        [HttpPut("{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCartById(Guid cartId, CartRequestDTO cartDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cart = await _cartService.GetCartById(cartId);

            if (cart == null)
            {
                return NotFound();
            }

            cart.TotalAmount = cartDTO.TotalAmount;

            var action = await _cartService.UpdateCart(cart);
            if (action == null)
            {
                return BadRequest();
            }

            var cartResponseDto = new CartResponseDTO
            {
                CartID = cart.CartID,
                UserID = cart.UserID,
                TotalAmount = cart.TotalAmount
            };

            return Ok(cartResponseDto);
        }

        /// <summary>
        /// Deletes a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <response code="204">If the cart was successfully deleted.</response>
        /// <response code="404">If the cart is not found.</response>
        [HttpDelete("{cartId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCartById(Guid cartId)
        {
            var cart = await _cartService.GetCartById(cartId);

            if (cart == null)
            {
                return NotFound();
            }

            await _cartService.DeleteCart(cart.CartID);

            return NoContent();
        }

        /// <summary>
        /// Adds an item to a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart to add the item to.</param>
        /// <param name="cartItemDTO">The cart item data transfer object containing product ID, quantity, and price.</param>
        /// <returns>A newly created cart item.</returns>
        /// <response code="201">Returns the newly created cart item.</response>
        /// <response code="400">If the cart item data is invalid.</response>
        [HttpPost("{cartId}/items")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CartItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCartItem(Guid cartId, CartItemRequestDTO cartItemDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cartItem = new CartItem
            {
                CartID = cartId,
                ProductID = cartItemDTO.ProductID,
                Quantity = cartItemDTO.Quantity,
                Price = cartItemDTO.Price
            };

            var action = await _cartItemService.CreateCartItem(cartItem);
            if (action == null)
            {
                return BadRequest();
            }

            var cartItemResponseDTO = new CartItemResponseDTO
            {
                CartItemID = cartItem.CartItemID,
                CartID = cartItem.CartID,
                ProductID = cartItem.ProductID,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            };

            return CreatedAtAction(nameof(CreateCartItem), cartItemResponseDTO);
        }

        /// <summary>
        /// Gets all items in a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart to get items from.</param>
        /// <returns>A list of cart items.</returns>
        /// <response code="200">Returns the list of cart items.</response>
        [HttpGet("{cartId}/items")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CartItemResponseDTO>))]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartItemResponseDTO>> GetCartItemById(Guid cartId, Guid itemId)
        {
            var cartItem = await _cartItemService.GetCartItemById(itemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            var cartItemResponseDto = new CartItemResponseDTO
            {
                CartItemID = cartItem.CartItemID,
                CartID = cartItem.CartID,
                ProductID = cartItem.ProductID,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            };

            return Ok(cartItemResponseDto);
        }

        /// <summary>
        /// Updates an item in a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <param name="itemId">The ID of the cart item to update.</param>
        /// <param name="cartItemDTO">The cart item data transfer object containing updated product ID, quantity, and price.</param>
        /// <response code="204">If the cart item was successfully updated.</response>
        /// <response code="404">If the cart item is not found.</response>
        /// <response code="400">If the cart item data is invalid.</response>
        [HttpPut("{cartId}/items/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCartItem(Guid cartId, Guid itemId, CartItemRequestDTO cartItemDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cartItem = await _cartItemService.GetCartItemById(itemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.CartID = cartItemDTO.CartID;
            cartItem.ProductID = cartItemDTO.ProductID;
            cartItem.Quantity = cartItemDTO.Quantity;
            cartItem.Price = cartItemDTO.Price;

            var action = await _cartItemService.UpdateCartItem(cartItem);
            if (action == null)
            {
                return BadRequest();
            }

            var cartItemResponseDto = new CartItemResponseDTO
            {
                CartItemID = cartItem.CartItemID,
                CartID = cartItem.CartID,
                ProductID = cartItem.ProductID,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            };

            return Ok(cartItemResponseDto);
        }

        /// <summary>
        /// Deletes an item from a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <param name="itemId">The ID of the cart item to delete.</param>
        /// <response code="204">If the cart item was successfully deleted.</response>
        /// <response code="404">If the cart item is not found.</response>
        [HttpDelete("{cartId}/items/{itemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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