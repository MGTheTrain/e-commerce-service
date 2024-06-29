// <copyright file="ProductService.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.ProductManagement;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ProductService : IProductService
    {
        private readonly PsqlDbContext context;
        private readonly ILogger<ProductService> logger;

        public ProductService(PsqlDbContext context, ILogger<ProductService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<Product?> GetProductById(Guid productId)
        {
            this.logger.LogInformation("Fetching product by ID: {ProductId}", productId);
            try
            {
                return await this.context.Products.FindAsync(productId);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching product by ID: {ProductId}", productId);
                return await Task.FromResult<Product?>(null);
            }
        }

        public async Task<IEnumerable<Product>?> GetAllProducts()
        {
            this.logger.LogInformation("Fetching all products");
            try
            {
                return await this.context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching all products");
                return await Task.FromResult<IEnumerable<Product>?>(null);
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(Guid categoryId)
        {
            this.logger.LogInformation("Fetching products by Category ID: {CategoryId}", categoryId);
            try
            {
                return await this.context.Products.Where(p => p.CategoryID == categoryId).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching products by Category ID: {CategoryId}", categoryId);
                return await Task.FromResult<IEnumerable<Product>?>(null);
            }
        }

        public async Task<Product?> CreateProduct(Product product)
        {
            this.logger.LogInformation("Creating new product: {ProductName}", product.Name);
            try
            {
                this.context.Products.Add(product);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Product created successfully: {ProductId}", product.ProductID);
                return await Task.FromResult<Product?>(product);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating product: {ProductName}", product.Name);
                return await Task.FromResult<Product?>(null);
            }
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            this.logger.LogInformation("Updating product: {ProductId}", product.ProductID);
            try
            {
                this.context.Products.Update(product);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Product updated successfully: {ProductId}", product.ProductID);
                return await Task.FromResult<Product?>(product);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating product: {ProductId}", product.ProductID);
                return await Task.FromResult<Product?>(null);
            }
        }

        public async Task DeleteProduct(Guid productId)
        {
            this.logger.LogInformation("Deleting product: {ProductId}", productId);
            try
            {
                var product = await this.context.Products.FindAsync(productId);
                if (product != null)
                {
                    this.context.Products.Remove(product);
                    await this.context.SaveChangesAsync();
                    this.logger.LogInformation("Product deleted successfully: {ProductId}", productId);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting product: {ProductId}", productId);
            }
        }
    }
}
