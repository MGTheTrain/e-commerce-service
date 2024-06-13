using Microsoft.AspNetCore.Mvc;
using Mgtt.ECom.Application.DTOs.Order;
using Mgtt.ECom.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mgtt.ECom.Web.v1.OrderManagement.Controllers
{
    [Route("api/v1/orderItems")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItem(OrderItemRequestDTO orderItemDTO)
        {
            var orderItem = new OrderItem
            {
                OrderID = orderItemDTO.OrderID,
                ProductID = orderItemDTO.ProductID,
                Quantity = orderItemDTO.Quantity,
                Price = orderItemDTO.Price
            };

            await _orderItemService.CreateOrderItem(orderItem);

            return CreatedAtAction(nameof(GetOrderItemById), new { orderItemId = orderItem.OrderItemID }, orderItem);
        }

        [HttpGet("{orderItemId}")]
        public async Task<ActionResult<OrderItemResponseDTO>> GetOrderItemById(Guid orderItemId)
        {
            var orderItem = await _orderItemService.GetOrderItemById(orderItemId);

            if (orderItem == null)
            {
                return NotFound();
            }

            var orderItemDTO = new OrderItemResponseDTO
            {
                OrderItemID = orderItem.OrderItemID,
                OrderID = orderItem.OrderID,
                ProductID = orderItem.ProductID,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price
            };

            return Ok(orderItemDTO);
        }

        [HttpGet("order/{orderId}")]
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

        [HttpPut("{orderItemId}")]
        public async Task<IActionResult> UpdateOrderItem(Guid orderItemId, OrderItemRequestDTO orderItemDTO)
        {
            var orderItem = await _orderItemService.GetOrderItemById(orderItemId);

            if (orderItem == null)
            {
                return NotFound();
            }

            orderItem.OrderID = orderItemDTO.OrderID;
            orderItem.ProductID = orderItemDTO.ProductID;
            orderItem.Quantity = orderItemDTO.Quantity;
            orderItem.Price = orderItemDTO.Price;

            await _orderItemService.UpdateOrderItem(orderItem);

            return NoContent();
        }

        [HttpDelete("{orderItemId}")]
        public async Task<IActionResult> DeleteOrderItem(Guid orderItemId)
        {
            var orderItem = await _orderItemService.GetOrderItemById(orderItemId);

            if (orderItem == null)
            {
                return NotFound();
            }

            await _orderItemService.DeleteOrderItem(orderItemId);

            return NoContent();
        }
    }
}
