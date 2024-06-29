// <copyright file="ICartService.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.ShoppingCart;

public interface ICartService
{
    Task<IEnumerable<Cart>?> GetAllCarts();

    Task<Cart?> GetCartById(Guid cartId);

    Task<Cart?> CreateCart(Cart cart);

    Task<Cart?> UpdateCart(Cart cart);

    Task DeleteCart(Guid cartId);
}
