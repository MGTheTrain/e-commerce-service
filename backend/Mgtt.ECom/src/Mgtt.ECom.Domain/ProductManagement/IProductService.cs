// <copyright file="IProductService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;

namespace Mgtt.ECom.Domain.ProductManagement;

public interface IProductService
{
    Task<Product?> GetProductById(Guid productId);

    Task<IEnumerable<Product>?> GetAllProducts(
            int pageNumber = 1, 
            int pageSize = 10, 
            string? category = null, 
            string? name = null, 
            float? minPrice = null, 
            float? maxPrice = null);

    Task<IEnumerable<Product>?> GetProductsByUserId(string userId);

    Task<Product?> CreateProduct(Product product, IFormFile file);

    Task<Product?> UpdateProduct(Product product, IFormFile file);

    Task DeleteProduct(Guid productId);
}
