using Microsoft.AspNetCore.Mvc;
using Mgtt.ECom.Application.DTOs.Order;
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

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderRequestDTO orderDTO)
        {
            var order = new Order
            {
                UserID = orderDTO.UserID,
                OrderDate = orderDTO.OrderDate,
                TotalAmount = orderDTO.TotalAmount,
                OrderStatus = orderDTO.OrderStatus
            };

            await _orderService.CreateOrder(order);

            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.OrderID }, order);
        }

        [HttpGet("{orderId}")]
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

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByUserId(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserId(userId);
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

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(Guid orderId, OrderRequestDTO orderDTO)
        {
            var order = await _orderService.GetOrderById(orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.UserID = orderDTO.UserID;
            order.OrderDate = orderDTO.OrderDate;
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
    }
}
