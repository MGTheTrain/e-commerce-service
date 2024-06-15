using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mgtt.ECom.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mgtt.ECom.Web.v1.OrderManagement.DTOs;

namespace Mgtt.ECom.Web.v1.OrderManagement.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        public OrderController(IOrderService orderService, IOrderItemService orderItemService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderDTO">The order data transfer object containing order details.</param>
        /// <returns>The newly created order.</returns>
        /// <response code="201">Returns the newly created order.</response>
        /// <response code="400">If the order data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder(OrderRequestDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = new Order
            {
                UserID = orderDTO.UserID,
                OrderDate = DateTime.UtcNow,
                TotalAmount = orderDTO.TotalAmount,
                OrderStatus = orderDTO.OrderStatus
            };

            var action = await _orderService.CreateOrder(order);
            if (action == null)
            {
                return BadRequest();
            }

            var orderResponseDTO = new OrderResponseDTO
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus
            };

            return CreatedAtAction(nameof(CreateOrder),orderResponseDTO);
        }

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        /// <response code="200">Returns a list of all orders.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderResponseDTO>))]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrders()
        {
            var orders = await _orderService.GetOrdersByUserId(Guid.NewGuid()); // Example for demo
            var orderDTOs = new List<OrderResponseDTO>();

            foreach (var order in orders)
            {
                orderDTOs.Add(new OrderResponseDTO
                {
                    OrderID = order.OrderID,
                    UserID = order.UserID,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderStatus = order.OrderStatus
                });
            }

            return Ok(orderDTOs);
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>The order with the specified ID.</returns>
        /// <response code="200">Returns the order with the specified ID.</response>
        /// <response code="404">If the order is not found.</response>
        [HttpGet("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById(Guid orderId)
        {
            var order = await _orderService.GetOrderById(orderId);

            if (order == null)
            {
                return NotFound();
            }

            var orderDTO = new OrderResponseDTO
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus
            };

            return Ok(orderDTO);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderRequestDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrder(Guid orderId, OrderRequestDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _orderService.GetOrderById(orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.TotalAmount = orderDTO.TotalAmount;
            order.OrderStatus = orderDTO.OrderStatus;

            var action = await _orderService.UpdateOrder(order);
            if (action == null)
            {
                return BadRequest();
            }

            var orderResponseDTO = new OrderResponseDTO
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus
            };

            return Ok(orderResponseDTO);
        }

        /// <summary>
        /// Deletes an order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to delete.</param>
        /// <returns>No content response if successful.</returns>
        /// <response code="204">If the order was successfully deleted.</response>
        /// <response code="404">If the order is not found.</response>
        [HttpDelete("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var order = await _orderService.GetOrderById(orderId);

            if (order == null)
            {
                return NotFound();
            }

            await _orderService.DeleteOrder(orderId);

            return NoContent();
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrderItem(Guid orderId, OrderItemRequestDTO orderItemDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderItem = new OrderItem
            {
                OrderID = orderId,
                ProductID = orderItemDTO.ProductID,
                Quantity = orderItemDTO.Quantity,
                Price = orderItemDTO.Price
            };

            var action = await _orderItemService.CreateOrderItem(orderItem);
            if (action == null)
            {
                return BadRequest();
            }

            var orderItemResponseDTO = new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
                ProductID = orderItem.ProductID,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price
            };

            return CreatedAtAction(nameof(CreateOrderItem),orderItemResponseDTO);
        }

        /// <summary>
        /// Retrieves all order items belonging to a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>A list of all order items within the specified order.</returns>
        /// <response code="200">Returns a list of all order items within the specified order.</response>
        /// <response code="404">If the order items are not found.</response>
        [HttpGet("{orderId}/items")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderItemResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderItemResponseDTO>>> GetOrderItemsByOrderId(Guid orderId)
        {
            var orderItems = await _orderItemService.GetOrderItemsByOrderId(orderId);
            var orderItemDTOs = new List<OrderItemResponseDTO>();

            foreach (var orderItem in orderItems)
            {
                orderItemDTOs.Add(new OrderItemResponseDTO
                {
                    OrderItemID = orderItem.OrderItemID,
                    OrderID = orderItem.OrderID,
                    ProductID = orderItem.ProductID,
                    Quantity = orderItem.Quantity,
                    Price = orderItem.Price
                });
            }

            return Ok(orderItemDTOs);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderItemResponseDTO>> GetOrderItemById(Guid orderId, Guid itemId)
        {
            var orderItem = await _orderItemService.GetOrderItemById(itemId);
            if (orderItem == null)
            {
                return NotFound();
            }

            var orderItemResponseDTO = new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
                ProductID = orderItem.ProductID,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price
            };

            return Ok(orderItemResponseDTO);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderItemResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderItem(Guid orderId, Guid itemId, OrderItemRequestDTO orderItemDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderItem = await _orderItemService.GetOrderItemById(itemId);

            if (orderItem == null)
            {
                return NotFound();
            }

            orderItem.ProductID = orderItemDTO.ProductID;
            orderItem.Quantity = orderItemDTO.Quantity;
            orderItem.Price = orderItemDTO.Price;

            var action = await _orderItemService.UpdateOrderItem(orderItem);
            if (action == null)
            {
                return BadRequest();
            }

            var orderItemResponseDTO = new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
                ProductID = orderItem.ProductID,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price
            };

            return Ok(orderItemResponseDTO);
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrderItem(Guid orderId, Guid itemId)
        {
            var orderItem = await _orderItemService.GetOrderItemById(itemId);

            if (orderItem == null)
            {
                return NotFound();
            }

            await _orderItemService.DeleteOrderItem(itemId);

            return NoContent();
        }
    }
}