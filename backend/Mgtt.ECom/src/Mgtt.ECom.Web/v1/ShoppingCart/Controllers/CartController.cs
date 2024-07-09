// <copyright file="CartController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.ShoppingCart.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.ShoppingCart;
    using Mgtt.ECom.Web.V1.ShoppingCart.DTOs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using static System.Collections.Specialized.BitVector32;

    [Route("api/v1/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly ICartItemService cartItemService;

        public CartController(ICartService cartService, ICartItemService cartItemService)
        {
            this.cartService = cartService;
            this.cartItemService = cartItemService;
        }

        /// <summary>
        /// Determines if the user has the "manage:carts" or "manage:own-cart" permission, and if applicable, validates the user's carts.
        /// </summary>
        /// <param name="isCreateOperation">Indicates whether the operation is a creation operation.</param>
        /// <param name="cartId">The cart id to check against.</param>
        /// <returns>True if the user has the required permissions and, if necessary, has valid carts; otherwise, false.</returns>
        private async Task<string?> CheckManageOwnCartPermission(bool isCreateOperation, Guid cartId)
        {
            var permissionsClaims = this.User.FindAll("permissions");
            var userIdClaim = this.User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value;

            if (userId == null)
            {
                return null;
            }

            if (permissionsClaims.Any(x => x.Value.Split(' ').Contains("manage:carts")))
            {
                if (!isCreateOperation && cartId != Guid.Empty)
                {
                    var cart = await this.cartService.GetCartById(cartId);
                    return cart?.UserID ?? userId;
                }
                return userId;
            }

            if (permissionsClaims.Any(x => x.Value.Split(' ').Contains("manage:own-cart")))
            {
                if (!isCreateOperation && cartId != Guid.Empty)
                {
                    var userCarts = await this.cartService.GetCartsByUserId(userId);
                    return userCarts?.Any(x => x.CartID == cartId) == true ? userId : null;
                }
                return userId;
            }

            return null;
        }

        /// <summary>
        /// Creates a new shopping cart.
        /// </summary>
        /// <param name="cartDTO">The cart data transfer object containing user ID and total amount.</param>
        /// <returns>A newly created cart.</returns>
        /// <response code="201">Returns the newly created cart.</response>
        /// <response code="400">If the cart data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpPost]
        [Authorize("manage:carts-and-own-cart")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CartResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateCart(CartRequestDTO cartDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = true;
            var userId = await this.CheckManageOwnCartPermission(isCreateOperation, Guid.Empty);
            if (userId == null)
            {
                return this.Forbid();
            }

            var cart = new Cart
            {
                UserID = userId!,
                TotalAmount = cartDTO.TotalAmount,
            };

            var action = await this.cartService.CreateCart(cart);
            if (action == null)
            {
                return this.BadRequest();
            }

            var cartResponseDTO = new CartResponseDTO
            {
                CartID = cart.CartID,
                UserID = cart.UserID,
                TotalAmount = cart.TotalAmount,
            };

            return this.CreatedAtAction(nameof(this.CreateCart), cartResponseDTO);
        }

        /// <summary>
        /// Gets a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <returns>The cart with the specified ID.</returns>
        /// <response code="200">Returns the cart with the specified ID.</response>
        /// <response code="404">If the cart is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpGet("{cartId}")]
        [Authorize("manage:carts-and-own-cart")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CartResponseDTO>> GetCartById(Guid cartId)
        {
            var isCreateOperation = false;
            var userId = await this.CheckManageOwnCartPermission(isCreateOperation, cartId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var cart = await this.cartService.GetCartById(cartId);

            if (cart == null)
            {
                return this.NotFound();
            }

            var cartResponseDto = new CartResponseDTO // utilize auto-mapper for optimization
            {
                CartID = cart.CartID,
                UserID = cart.UserID,
                TotalAmount = cart.TotalAmount,
            };

            return this.Ok(cartResponseDto);
        }

        /// <summary>
        /// Retrieves all carts.
        /// </summary>
        /// <returns>A list of all carts.</returns>
        /// <response code="200">Returns a list of all carts.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpGet]
        [Authorize("manage:carts-and-own-cart")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CartResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<CartResponseDTO>>> GetAllCarts()
        {
            var isCreateOperation = false;
            var userId = await this.CheckManageOwnCartPermission(isCreateOperation, Guid.Empty);
            if (userId == null)
            {
                return this.Forbid();
            }

            var carts = await this.cartService.GetAllCarts();
            var cartDTOs = new List<CartResponseDTO>();

            foreach (var cart in carts)
            {
                cartDTOs.Add(new CartResponseDTO
                {
                    CartID = cart.CartID,
                    UserID = cart.UserID,
                    TotalAmount = cart.TotalAmount,
                });
            }

            return this.Ok(cartDTOs);
        }

        /// <summary>
        /// Updates an existing cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart user.</param>
        /// <param name="cartDTO">The cart data transfer object containing the updated total amount.</param>
        /// <response code="204">If the cart was successfully updated.</response>
        /// <response code="404">If the cart is not found.</response>
        /// <response code="400">If the cart data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut("{cartId}")]
        [Authorize("manage:carts-and-own-cart")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateCartById(Guid cartId, CartRequestDTO cartDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = false;
            var userId = await this.CheckManageOwnCartPermission(isCreateOperation, cartId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var cart = await this.cartService.GetCartById(cartId);

            if (cart == null)
            {
                return this.NotFound();
            }

            cart.TotalAmount = cartDTO.TotalAmount;

            var action = await this.cartService.UpdateCart(cart);
            if (action == null)
            {
                return this.BadRequest();
            }

            var cartResponseDto = new CartResponseDTO
            {
                CartID = cart.CartID,
                UserID = cart.UserID,
                TotalAmount = cart.TotalAmount,
            };

            return this.Ok(cartResponseDto);
        }

        /// <summary>
        /// Deletes a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <response code="204">If the cart was successfully deleted.</response>
        /// <response code="404">If the cart is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpDelete("{cartId}")]
        [Authorize("manage:carts-and-own-cart")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteCartById(Guid cartId)
        {
            var isCreateOperation = false;
            var userId = await this.CheckManageOwnCartPermission(isCreateOperation, cartId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var cart = await this.cartService.GetCartById(cartId);

            if (cart == null)
            {
                return this.NotFound();
            }

            await this.cartService.DeleteCart(cart.CartID);

            return this.NoContent();
        }

        /// <summary>
        /// Adds an item to a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart to add the item to.</param>
        /// <param name="cartItemDTO">The cart item data transfer object containing product ID, quantity, and price.</param>
        /// <returns>A newly created cart item.</returns>
        /// <response code="201">Returns the newly created cart item.</response>
        /// <response code="400">If the cart item data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpPost("{cartId}/items")]
        [Authorize("manage:carts-and-own-cart")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CartItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateCartItem(Guid cartId, CartItemRequestDTO cartItemDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = false; // false because we are verifying the cart resource which consists of cart items
            var userId = await this.CheckManageOwnCartPermission(isCreateOperation, cartId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var cartItem = new CartItem
            {
                CartID = cartId,
                ProductID = cartItemDTO.ProductID,
                Quantity = cartItemDTO.Quantity,
                Price = cartItemDTO.Price,
            };

            var action = await this.cartItemService.CreateCartItem(cartItem);
            if (action == null)
            {
                return this.BadRequest();
            }

            var cartItemResponseDTO = new CartItemResponseDTO
            {
                CartItemID = cartItem.CartItemID,
                CartID = cartItem.CartID,
                ProductID = cartItem.ProductID,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
            };

            return this.CreatedAtAction(nameof(this.CreateCartItem), cartItemResponseDTO);
        }

        /// <summary>
        /// Gets all items in a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart to get items from.</param>
        /// <returns>A list of cart items.</returns>
        /// <response code="200">Returns the list of cart items.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpGet("{cartId}/items")]
        [Authorize("manage:carts-and-own-cart")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CartItemResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<CartItemResponseDTO>>> GetCartItemsByCartId(Guid cartId)
        {
            var isCreateOperation = false;
            var userId = await this.CheckManageOwnCartPermission(isCreateOperation, cartId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var cartItems = await this.cartItemService.GetCartItemsByCartId(cartId);
            var cartItemDTOs = new List<CartItemResponseDTO>();

            foreach (var cartItem in cartItems)
            {
                cartItemDTOs.Add(new CartItemResponseDTO
                {
                    CartItemID = cartItem.CartItemID,
                    CartID = cartItem.CartID,
                    ProductID = cartItem.ProductID,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                });
            }

            return this.Ok(cartItemDTOs);
        }

        /// <summary>
        /// Gets a specific item in a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <param name="itemId">The ID of the cart item.</param>
        /// <returns>The cart item with the specified ID.</returns>
        /// <response code="200">Returns the cart item with the specified ID.</response>
        /// <response code="404">If the cart item is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpGet("{cartId}/items/{itemId}")]
        [Authorize("manage:carts-and-own-cart")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CartItemResponseDTO>> GetCartItemById(Guid cartId, Guid itemId)
        {
            var isCreateOperation = false;
            var userId = await this.CheckManageOwnCartPermission(isCreateOperation, cartId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var cartItem = await this.cartItemService.GetCartItemById(itemId);

            if (cartItem == null)
            {
                return this.NotFound();
            }

            var cartItemResponseDto = new CartItemResponseDTO
            {
                CartItemID = cartItem.CartItemID,
                CartID = cartItem.CartID,
                ProductID = cartItem.ProductID,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
            };

            return this.Ok(cartItemResponseDto);
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
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut("{cartId}/items/{itemId}")]
        [Authorize("manage:carts-and-own-cart")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateCartItem(Guid cartId, Guid itemId, CartItemRequestDTO cartItemDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = false;
            var userId = await this.CheckManageOwnCartPermission(isCreateOperation, cartId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var cartItem = await this.cartItemService.GetCartItemById(itemId);

            if (cartItem == null)
            {
                return this.NotFound();
            }

            cartItem.CartID = cartItemDTO.CartID;
            cartItem.ProductID = cartItemDTO.ProductID;
            cartItem.Quantity = cartItemDTO.Quantity;
            cartItem.Price = cartItemDTO.Price;

            var action = await this.cartItemService.UpdateCartItem(cartItem);
            if (action == null)
            {
                return this.BadRequest();
            }

            var cartItemResponseDto = new CartItemResponseDTO
            {
                CartItemID = cartItem.CartItemID,
                CartID = cartItem.CartID,
                ProductID = cartItem.ProductID,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
            };

            return this.Ok(cartItemResponseDto);
        }

        /// <summary>
        /// Deletes an item from a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <param name="itemId">The ID of the cart item to delete.</param>
        /// <response code="204">If the cart item was successfully deleted.</response>
        /// <response code="404">If the cart item is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpDelete("{cartId}/items/{itemId}")]
        [Authorize("manage:carts-and-own-cart")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteCartItem(Guid cartId, Guid itemId)
        {
            var isCreateOperation = false;
            var userId = await this.CheckManageOwnCartPermission(isCreateOperation, cartId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var cartItem = await this.cartItemService.GetCartItemById(itemId);

            if (cartItem == null)
            {
                return this.NotFound();
            }

            await this.cartItemService.DeleteCartItem(itemId);

            return this.NoContent();
        }
    }
}