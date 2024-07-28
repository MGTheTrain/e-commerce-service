// <copyright file="OrderController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.OrderManagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.OrderManagement;
    using Mgtt.ECom.Web.V1.OrderManagement.DTOs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IOrderItemService orderItemService;

        public OrderController(IOrderService orderService, IOrderItemService orderItemService)
        {
            this.orderService = orderService;
            this.orderItemService = orderItemService;
        }

        /// <summary>
        /// Determines if the user has the "manage:orders" or "manage:own-order" permission, and if applicable, validates the user's orders.
        /// </summary>
        /// <param name="isCreateOperation">Indicates whether the operation is a creation operation.</param>
        /// <param name="orderId">The order id to check against.</param>
        /// <returns>True if the user has the required permissions and, if necessary, has valid orders; otherwise, false.</returns>
        private async Task<string?> VerifyUserPermissionForOrder(bool isCreateOperation, string orderId)
        {
            var permissionsClaims = this.User.FindAll("permissions");
            var userIdClaim = this.User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value;

            if (userId == null)
            {
                return null;
            }

            if (permissionsClaims.Any(x => x.Value.Split(' ').Contains("manage:orders")))
            {
                if (!isCreateOperation && orderId != string.Empty)
                {
                    var order = await this.orderService.GetOrderById(orderId);
                    return order?.UserID ?? userId;
                }

                return userId;
            }

            if (permissionsClaims.Any(x => x.Value.Split(' ').Contains("manage:own-order")))
            {
                if (!isCreateOperation && orderId != string.Empty)
                {
                    var userOrders = await this.orderService.GetOrdersByUserId(userId);
                    return userOrders?.Any(x => x.OrderID == orderId) == true ? userId : null;
                }

                return userId;
            }

            return null;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderDTO">The order data transfer object containing order details.</param>
        /// <returns>The newly created order.</returns>
        /// <response code="201">Returns the newly created order.</response>
        /// <response code="400">If the order data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpPost]
        [Authorize(Policy = "manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateOrder(OrderRequestDTO orderDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = true;
            var userId = await this.VerifyUserPermissionForOrder(isCreateOperation, string.Empty);
            if (userId == null)
            {
                return this.Forbid();
            }

            var order = new Order
            {
                UserID = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = orderDTO.TotalAmount,
                OrderStatus = orderDTO.OrderStatus,
                CurrencyCode = orderDTO.CurrencyCode,
                ReferenceId = orderDTO.ReferenceId,
                AddressLine1 = orderDTO.AddressLine1,
                AddressLine2 = orderDTO.AddressLine2,
                AdminArea2 = orderDTO.AdminArea2,
                AdminArea1 = orderDTO.AdminArea1,
                PostalCode = orderDTO.PostalCode,
                CountryCode = orderDTO.CountryCode,
            };

            var action = await this.orderService.CreateOrder(order);
            if (action == null)
            {
                return this.BadRequest();
            }

            var orderResponseDTO = new OrderResponseDTO
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                CurrencyCode = order.CurrencyCode,
                ReferenceId = order.ReferenceId,
                AddressLine1 = order.AddressLine1,
                AddressLine2 = order.AddressLine2,
                AdminArea2 = order.AdminArea2,
                AdminArea1 = order.AdminArea1,
                PostalCode = order.PostalCode,
                CountryCode = order.CountryCode,
                CheckoutNowHref = order.CheckoutNowHref,
            };

            return this.CreatedAtAction(nameof(this.CreateOrder), orderResponseDTO);
        }

        /// <summary>
        /// Retrieves the order associated with a specific user.
        /// Explicitly checks whether a order belongs to a user by requiring a order id.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>The order by user id.</returns>
        /// <response code="200">Returns the order by user id.</response>
        [HttpGet("{orderId}/user")]
        [Authorize("manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponseDTO))]
        public async Task<ActionResult<OrderResponseDTO>> GetUserOrderById(string orderId)
        {
            var isCreateOperation = false;
            var userId = await this.VerifyUserPermissionForOrder(isCreateOperation, orderId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var orders = await this.orderService.GetOrdersByUserId(userId);
            var order = orders!.FirstOrDefault();

            if (order == null)
            {
                return this.NotFound();
            }

            var orderDTO = new OrderResponseDTO
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                CurrencyCode = order.CurrencyCode,
                ReferenceId = order.ReferenceId,
                AddressLine1 = order.AddressLine1,
                AddressLine2 = order.AddressLine2,
                AdminArea2 = order.AdminArea2,
                AdminArea1 = order.AdminArea1,
                PostalCode = order.PostalCode,
                CountryCode = order.CountryCode,
                CheckoutNowHref = order.CheckoutNowHref,
            };

            return this.Ok(orderDTO);
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>The order with the specified ID.</returns>
        /// <response code="200">Returns the order with the specified ID.</response>
        /// <response code="404">If the order is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpGet("{orderId}")]
        [Authorize("manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById(string orderId)
        {
            var order = await this.orderService.GetOrderById(orderId);

            if (order == null)
            {
                return this.NotFound();
            }

            var orderDTO = new OrderResponseDTO
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                CurrencyCode = order.CurrencyCode,
                ReferenceId = order.ReferenceId,
                AddressLine1 = order.AddressLine1,
                AddressLine2 = order.AddressLine2,
                AdminArea2 = order.AdminArea2,
                AdminArea1 = order.AdminArea1,
                PostalCode = order.PostalCode,
                CountryCode = order.CountryCode,
                CheckoutNowHref = order.CheckoutNowHref,
            };

            return this.Ok(orderDTO);
        }

        /// <summary>
        /// Retrieves the orders created for a specific user.
        /// </summary>
        /// <returns>A single order created for a authenticated user.</returns>
        /// <response code="200">Returns the order created for a authenticated user.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not authorized to access the resource.</response>
        [HttpGet("user")]
        [Authorize("manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersForUser()
        {
            var isCreateOperation = false;
            var userId = await this.VerifyUserPermissionForOrder(isCreateOperation, string.Empty);
            if (userId == null)
            {
                return this.Forbid();
            }

            var orders = await this.orderService.GetOrdersByUserId(userId);
            var orderDTOs = new List<OrderResponseDTO>();

            foreach (var order in orders!)
            {
                orderDTOs.Add(new OrderResponseDTO
                {
                    OrderID = order.OrderID,
                    UserID = order.UserID,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderStatus = order.OrderStatus,
                    CurrencyCode = order.CurrencyCode,
                    ReferenceId = order.ReferenceId,
                    AddressLine1 = order.AddressLine1,
                    AddressLine2 = order.AddressLine2,
                    AdminArea2 = order.AdminArea2,
                    AdminArea1 = order.AdminArea1,
                    PostalCode = order.PostalCode,
                    CountryCode = order.CountryCode,
                    CheckoutNowHref = order.CheckoutNowHref,
                });
            }

            return this.Ok(orderDTOs);
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="orderId">The ID of the order to update.</param>
        /// <param name="orderDTO">The order data transfer object containing updated order details.</param>
        /// <returns>No content response if successful.</returns>
        /// <response code="204">If the order was successfully updated.</response>
        /// <response code="400">If the order data is invalid.</response>
        /// <response code="404">If the order is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpPut("{orderId}")]
        [Authorize(Policy = "manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderRequestDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateOrder(string orderId, OrderRequestDTO orderDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = false;
            var userId = await this.VerifyUserPermissionForOrder(isCreateOperation, orderId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var order = await this.orderService.GetOrderById(orderId);

            if (order == null)
            {
                return this.NotFound();
            }

            order.TotalAmount = orderDTO.TotalAmount;
            order.OrderStatus = orderDTO.OrderStatus;

            var action = await this.orderService.UpdateOrder(order);
            if (action == null)
            {
                return this.BadRequest();
            }

            var orderResponseDTO = new OrderResponseDTO
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                CurrencyCode = order.CurrencyCode,
                ReferenceId = order.ReferenceId,
                AddressLine1 = order.AddressLine1,
                AddressLine2 = order.AddressLine2,
                AdminArea2 = order.AdminArea2,
                AdminArea1 = order.AdminArea1,
                PostalCode = order.PostalCode,
                CountryCode = order.CountryCode,
                CheckoutNowHref = order.CheckoutNowHref,
            };

            return this.Ok(orderResponseDTO);
        }

        /// <summary>
        /// Deletes an order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to delete.</param>
        /// <returns>No content response if successful.</returns>
        /// <response code="204">If the order was successfully deleted.</response>
        /// <response code="404">If the order is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpDelete("{orderId}")]
        [Authorize(Policy = "manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            var isCreateOperation = false;
            var userId = await this.VerifyUserPermissionForOrder(isCreateOperation, orderId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var order = await this.orderService.GetOrderById(orderId);

            if (order == null)
            {
                return this.NotFound();
            }

            await this.orderService.DeleteOrder(orderId);

            return this.NoContent();
        }

        /// <summary>
        /// Creates a new order item within an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to which the item belongs.</param>
        /// <param name="orderItemDTO">The order item data transfer object containing item details.</param>
        /// <returns>The newly created order item.</returns>
        /// <response code="201">Returns the newly created order item.</response>
        /// <response code="400">If the order item data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpPost("{orderId}/items")]
        [Authorize(Policy = "manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateOrderItem(string orderId, OrderItemRequestDTO orderItemDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var order = await this.orderService.GetOrderById(orderId);
            if (order == null)
            {
                return this.BadRequest();
            }

            var isCreateOperation = false; // false because we are verifying the order resource which consists of order items
            var userId = await this.VerifyUserPermissionForOrder(isCreateOperation, orderId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var orderItem = new OrderItem
            {
                OrderID = orderId,
                ProductID = orderItemDTO.ProductID,
                UserID = userId,
                Quantity = orderItemDTO.Quantity,
                Price = orderItemDTO.Price,
            };

            var action = await this.orderItemService.CreateOrderItem(orderItem);
            if (action == null)
            {
                return this.BadRequest();
            }

            var orderItemResponseDTO = new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
                UserID = orderItem.UserID,
                ProductID = orderItem.ProductID,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price,
            };

            return this.CreatedAtAction(nameof(this.CreateOrderItem), orderItemResponseDTO);
        }

        /// <summary>
        /// Retrieves all order items belonging to a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>A list of all order items within the specified order.</returns>
        /// <response code="200">Returns a list of all order items within the specified order.</response>
        /// <response code="404">If the order items are not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpGet("{orderId}/items")]
        [Authorize(Policy = "manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderItemResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<OrderItemResponseDTO>>> GetOrderItemsByOrderId(string orderId)
        {
            var isCreateOperation = false;
            var userId = await this.VerifyUserPermissionForOrder(isCreateOperation, orderId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var order = await this.orderService.GetOrderById(orderId);
            if (order == null)
            {
                return this.BadRequest();
            }

            var orderItems = await this.orderItemService.GetOrderItemsByOrderId(orderId);
            var orderItemDTOs = new List<OrderItemResponseDTO>();

            foreach (var orderItem in orderItems)
            {
                orderItemDTOs.Add(new OrderItemResponseDTO
                {
                    OrderItemID = orderItem.OrderItemID,
                    OrderID = orderItem.OrderID,
                    ProductID = orderItem.ProductID,
                    UserID = orderItem.UserID,
                    Quantity = orderItem.Quantity,
                    Price = orderItem.Price,
                });
            }

            return this.Ok(orderItemDTOs);
        }

        /// <summary>
        /// Retrieves an order item by its ID within a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <param name="itemId">The ID of the order item.</param>
        /// <returns>The order item with the specified ID within the specified order.</returns>
        /// <response code="200">Returns the order item with the specified ID within the specified order.</response>
        /// <response code="404">If the order item is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpGet("{orderId}/items/{itemId}")]
        [Authorize(Policy = "manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<OrderItemResponseDTO>> GetOrderItemById(string orderId, Guid itemId)
        {
            var isCreateOperation = false;
            var userId = await this.VerifyUserPermissionForOrder(isCreateOperation, orderId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var order = await this.orderService.GetOrderById(orderId);
            if (order == null)
            {
                return this.BadRequest();
            }

            var orderItem = await this.orderItemService.GetOrderItemById(itemId);
            if (orderItem == null)
            {
                return this.NotFound();
            }

            var orderItemResponseDTO = new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
                ProductID = orderItem.ProductID,
                UserID = orderItem.UserID,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price,
            };

            return this.Ok(orderItemResponseDTO);
        }

        /// <summary>
        /// Gets a order item for a user.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>The order iem for the specified user.</returns>
        /// <response code="200">Returns the order with the specified ID.</response>
        /// <response code="404">If the order is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpGet("{orderId}/products/{productId}/item")]
        [Authorize("manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<OrderItemResponseDTO>> GetOrderItemForUser(string orderId, Guid productId)
        {
            var isCreateOperation = false;
            var userId = await this.VerifyUserPermissionForOrder(isCreateOperation, orderId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var orderItem = await this.orderItemService.GetOrderItemByProductAndUserId(productId, userId);

            if (orderItem == null)
            {
                return this.NotFound();
            }

            var orderItemResponseDto = new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
                ProductID = orderItem.ProductID,
                UserID = orderItem.UserID,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price,
            };

            return this.Ok(orderItemResponseDto);
        }

        /// <summary>
        /// Updates an existing order item within a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order to which the item belongs.</param>
        /// <param name="itemId">The ID of the order item to update.</param>
        /// <param name="orderItemDTO">The order item data transfer object containing updated item details.</param>
        /// <returns>No content response if successful.</returns>
        /// <response code="204">If the order item was successfully updated.</response>
        /// <response code="400">If the order item data is invalid.</response>
        /// <response code="404">If the order item is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpPut("{orderId}/items/{itemId}")]
        [Authorize(Policy = "manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateOrderItem(string orderId, Guid itemId, OrderItemRequestDTO orderItemDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = false;
            var userId = await this.VerifyUserPermissionForOrder(isCreateOperation, orderId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var order = await this.orderService.GetOrderById(orderId);
            if (order == null)
            {
                return this.NotFound();
            }

            var orderItem = await this.orderItemService.GetOrderItemById(itemId);

            if (orderItem == null)
            {
                return this.NotFound();
            }

            orderItem.ProductID = orderItemDTO.ProductID;
            orderItem.Quantity = orderItemDTO.Quantity;
            orderItem.Price = orderItemDTO.Price;

            // orderItem.UserID = orderItem.UserID;
            var action = await this.orderItemService.UpdateOrderItem(orderItem);
            if (action == null)
            {
                return this.BadRequest();
            }

            var orderItemResponseDTO = new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
                ProductID = orderItem.ProductID,
                UserID = orderItem.UserID,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price,
            };

            return this.Ok(orderItemResponseDTO);
        }

        /// <summary>
        /// Deletes an order item within a specific order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to which the item belongs.</param>
        /// <param name="itemId">The ID of the order item to delete.</param>
        /// <returns>No content response if successful.</returns>
        /// <response code="204">If the order item was successfully deleted.</response>
        /// <response code="404">If the order item is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpDelete("{orderId}/items/{itemId}")]
        [Authorize(Policy = "manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteOrderItem(string orderId, Guid itemId)
        {
            var isCreateOperation = false;
            var userId = await this.VerifyUserPermissionForOrder(isCreateOperation, orderId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var order = await this.orderService.GetOrderById(orderId);
            if (order == null)
            {
                return this.NotFound();
            }

            var orderItem = await this.orderItemService.GetOrderItemById(itemId);

            if (orderItem == null)
            {
                return this.NotFound();
            }

            await this.orderItemService.DeleteOrderItem(itemId);

            return this.NoContent();
        }
    }
}