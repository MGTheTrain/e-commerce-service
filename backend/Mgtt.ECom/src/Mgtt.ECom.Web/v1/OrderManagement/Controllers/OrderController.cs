using Microsoft.AspNetCore.Mvc;
using Mgtt.ECom.Application.DTOs.OrderManagement;
using Mgtt.ECom.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderRequestDTO orderDTO)
        {
            var order = new Order
            {
                UserID = orderDTO.UserID,
                OrderDate = DateTime.UtcNow,
                TotalAmount = orderDTO.TotalAmount,
                OrderStatus = orderDTO.OrderStatus
            };

            await _orderService.CreateOrder(order);

            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.OrderID }, new OrderResponseDTO
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus
            });
        }

        [HttpGet]
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

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById(Guid orderId)
        {
            var order = await _orderService.GetOrderById(orderId);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(new OrderResponseDTO
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus
            });
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(Guid orderId, OrderRequestDTO orderDTO)
        {
            var order = await _orderService.GetOrderById(orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.TotalAmount = orderDTO.TotalAmount;
            order.OrderStatus = orderDTO.OrderStatus;

            await _orderService.UpdateOrder(order);

            return NoContent();
        }

        [HttpDelete("{orderId}")]
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

        [HttpPost("{orderId}/items")]
        public async Task<IActionResult> CreateOrderItem(Guid orderId, OrderItemRequestDTO orderItemDTO)
        {
            var orderItem = new OrderItem
            {
                OrderID = orderId,
                ProductID = orderItemDTO.ProductID,
                Quantity = orderItemDTO.Quantity,
                Price = orderItemDTO.Price
            };

            await _orderItemService.CreateOrderItem(orderItem);

            return CreatedAtAction(nameof(GetOrderItemById), new { orderId, itemId = orderItem.OrderItemID }, new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
                ProductID = orderItem.ProductID,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price
            });
        }

        [HttpGet("{orderId}/items")]
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

        [HttpGet("{orderId}/items/{itemId}")]
        public async Task<ActionResult<OrderItemResponseDTO>> GetOrderItemById(Guid orderId, Guid itemId)
        {
            var orderItem = await _orderItemService.GetOrderItemById(itemId);

            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
                ProductID = orderItem.ProductID,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price
            });
        }

        [HttpPut("{orderId}/items/{itemId}")]
        public async Task<IActionResult> UpdateOrderItem(Guid orderId, Guid itemId, OrderItemRequestDTO orderItemDTO)
        {
            var orderItem = await _orderItemService.GetOrderItemById(itemId);

            if (orderItem == null)
            {
                return NotFound();
            }

            orderItem.ProductID = orderItemDTO.ProductID;
            orderItem.Quantity = orderItemDTO.Quantity;
            orderItem.Price = orderItemDTO.Price;

            await _orderItemService.UpdateOrderItem(orderItem);

            return NoContent();
        }

        [HttpDelete("{orderId}/items/{itemId}")]
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