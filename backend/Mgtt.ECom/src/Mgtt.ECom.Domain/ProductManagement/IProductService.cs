// <copyright file="IProductService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.ProductManagement;

public interface IProductService
{
    Task<Product?> GetProductById(Guid productId);

    Task<IEnumerable<Product>?> GetAllProducts();
    
    Task<IEnumerable<Product>?> GetProductsByUserId(string userId);

    Task<Product?> CreateProduct(Product product);

    Task<Product?> UpdateProduct(Product product);

    Task DeleteProduct(Guid productId);
}
