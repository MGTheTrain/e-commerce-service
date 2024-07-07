// <copyright file="OrderController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.OrderManagement.Controllers
{
    using System;
    using System.Collections.Generic;
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
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        public OrderController(IOrderService orderService, IOrderItemService orderItemService)
        {
            this._orderService = orderService;
            this._orderItemService = orderItemService;
        }

        /// <summary>
        /// Determines if the user has the "manage:orders" or "manage:own-order" permission, and if applicable, validates the user's orders.
        /// </summary>
        /// <param name="isCreateOperation">Indicates whether the operation is a creation operation.</param>
        /// <param name="orderId">The order id to check against.</param>
        /// <returns>True if the user has the required permissions and, if necessary, has valid orders; otherwise, false.</returns>
        private async Task<string?> CheckManageOwnOrderPermission(bool isCreateOperation, Guid orderId)
        {
            var permissionsClaims = this.User.FindAll("permissions");
            if (permissionsClaims.Any(x => x.Value.Split(' ').Contains("manage:orders")) || 
                permissionsClaims.Any(x => x.Value.Split(' ').Contains("manage:own-order")))
            {
                var userIdClaim = this.User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = userIdClaim!.Value;
                if (!isCreateOperation && orderId != Guid.Empty)
                {
                    var userCarts = await this._orderService.GetOrdersByUserId(userId);
                    if (userCarts!.Where(x => x.OrderID == orderId).FirstOrDefault() != null)
                    {
                        return userId;
                    }
                    return null;
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
        [HttpPost]
        [Authorize(Policy="manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder(OrderRequestDTO orderDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = true;
            var userId = await this.CheckManageOwnOrderPermission(isCreateOperation, Guid.Empty);
            if(userId == null)
            {
                return Forbid();
            }

            var order = new Order
            {
                UserID = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = orderDTO.TotalAmount,
                OrderStatus = orderDTO.OrderStatus,
            };

            var action = await this._orderService.CreateOrder(order);
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
            };

            return this.CreatedAtAction(nameof(this.CreateOrder), orderResponseDTO);
        }
        
        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>The order with the specified ID.</returns>
        /// <response code="200">Returns the order with the specified ID.</response>
        /// <response code="404">If the order is not found.</response>
        [HttpGet("{orderId}")]
        [Authorize("manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById(Guid orderId)
        {
            var order = await this._orderService.GetOrderById(orderId);

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
            };

            return this.Ok(orderDTO);
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
        [HttpPut("{orderId}")]
        [Authorize(Policy="manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderRequestDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrder(Guid orderId, OrderRequestDTO orderDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = false;
            var userId = await this.CheckManageOwnOrderPermission(isCreateOperation, orderId);
            if(userId == null)
            {
                return Forbid();
            }

            var order = await this._orderService.GetOrderById(orderId);

            if (order == null)
            {
                return this.NotFound();
            }

            order.TotalAmount = orderDTO.TotalAmount;
            order.OrderStatus = orderDTO.OrderStatus;

            var action = await this._orderService.UpdateOrder(order);
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
        [HttpDelete("{orderId}")]
        [Authorize(Policy="manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var isCreateOperation = false;
            var userId = await this.CheckManageOwnOrderPermission(isCreateOperation, orderId);
            if(userId == null)
            {
                return Forbid();
            }

            var order = await this._orderService.GetOrderById(orderId);

            if (order == null)
            {
                return this.NotFound();
            }

            await this._orderService.DeleteOrder(orderId);

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
        [HttpPost("{orderId}/items")]
        [Authorize(Policy="manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrderItem(Guid orderId, OrderItemRequestDTO orderItemDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = false; // false because we are verifying the order resource which consists of order items
            var userId = await this.CheckManageOwnOrderPermission(isCreateOperation, orderId);
            if(userId == null)
            {
                return Forbid();
            }

            var orderItem = new OrderItem
            {
                OrderID = orderId,
                ProductID = orderItemDTO.ProductID,
                Quantity = orderItemDTO.Quantity,
                Price = orderItemDTO.Price,
            };

            var action = await this._orderItemService.CreateOrderItem(orderItem);
            if (action == null)
            {
                return this.BadRequest();
            }

            var orderItemResponseDTO = new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
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
        [HttpGet("{orderId}/items")]
        [Authorize(Policy="manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderItemResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderItemResponseDTO>>> GetOrderItemsByOrderId(Guid orderId)
        {
            var isCreateOperation = false;
            var userId = await this.CheckManageOwnOrderPermission(isCreateOperation, orderId);
            if(userId == null)
            {
                return Forbid();
            }

            var orderItems = await this._orderItemService.GetOrderItemsByOrderId(orderId);
            var orderItemDTOs = new List<OrderItemResponseDTO>();

            foreach (var orderItem in orderItems)
            {
                orderItemDTOs.Add(new OrderItemResponseDTO
                {
                    OrderItemID = orderItem.OrderItemID,
                    OrderID = orderItem.OrderID,
                    ProductID = orderItem.ProductID,
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
        [HttpGet("{orderId}/items/{itemId}")]
        [Authorize(Policy="manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderItemResponseDTO>> GetOrderItemById(Guid orderId, Guid itemId)
        {
            var isCreateOperation = false;
            var userId = await this.CheckManageOwnOrderPermission(isCreateOperation, orderId);
            if(userId == null)
            {
                return Forbid();
            }

            var orderItem = await this._orderItemService.GetOrderItemById(itemId);
            if (orderItem == null)
            {
                return this.NotFound();
            }

            var orderItemResponseDTO = new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
                ProductID = orderItem.ProductID,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price,
            };

            return this.Ok(orderItemResponseDTO);
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
        [HttpPut("{orderId}/items/{itemId}")]
        [Authorize(Policy="manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderItem(Guid orderId, Guid itemId, OrderItemRequestDTO orderItemDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = false;
            var userId = await this.CheckManageOwnOrderPermission(isCreateOperation, orderId);
            if(userId == null)
            {
                return Forbid();
            }

            var orderItem = await this._orderItemService.GetOrderItemById(itemId);

            if (orderItem == null)
            {
                return this.NotFound();
            }

            orderItem.ProductID = orderItemDTO.ProductID;
            orderItem.Quantity = orderItemDTO.Quantity;
            orderItem.Price = orderItemDTO.Price;

            var action = await this._orderItemService.UpdateOrderItem(orderItem);
            if (action == null)
            {
                return this.BadRequest();
            }

            var orderItemResponseDTO = new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
                ProductID = orderItem.ProductID,
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
        [HttpDelete("{orderId}/items/{itemId}")]
        [Authorize(Policy="manage:orders-and-own-order")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrderItem(Guid orderId, Guid itemId)
        {
            var isCreateOperation = false;
            var userId = await this.CheckManageOwnOrderPermission(isCreateOperation, orderId);
            if(userId == null)
            {
                return Forbid();
            }

            var orderItem = await this._orderItemService.GetOrderItemById(itemId);

            if (orderItem == null)
            {
                return this.NotFound();
            }

            await this._orderItemService.DeleteOrderItem(itemId);

            return this.NoContent();
        }
    }
}