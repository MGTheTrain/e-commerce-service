// <copyright file="IOrderItemService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.OrderManagement;

public interface IOrderItemService
{
    Task<OrderItem?> GetOrderItemById(Guid orderItemId);

    Task<IEnumerable<OrderItem>?> GetOrderItemsByOrderId(Guid orderId);

    Task<OrderItem?> CreateOrderItem(OrderItem orderItem);

    Task<OrderItem?> UpdateOrderItem(OrderItem orderItem);

    Task DeleteOrderItem(Guid orderItemId);
}
