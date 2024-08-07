// <copyright file="IOrderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.OrderManagement;

public interface IOrderService
{
    Task<Order?> GetOrderById(string orderId);

    Task<IEnumerable<Order>?> GetOrdersByUserId(string userId);

    Task<Order?> CreateOrder(Order order);

    Task<Order?> UpdateOrder(Order order);

    Task DeleteOrder(string orderId);
}
