// <copyright file="ProductService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.ProductManagement;
    using Mgtt.ECom.Infrastructure.Connectors;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ProductService : IProductService
    {
        private readonly PsqlDbContext dbContext;

        private readonly IBlobConnector blobConnector;
        private readonly ILogger<ProductService> logger;

        public ProductService(PsqlDbContext dbContext, IBlobConnector blobConnector, ILogger<ProductService> logger)
        {
            this.dbContext = dbContext;
            this.blobConnector = blobConnector;
            this.logger = logger;
        }

        public async Task<Product?> GetProductById(Guid productId)
        {
            this.logger.LogInformation("Fetching product by ID: {ProductId}", productId);
            try
            {
                return await this.dbContext.Products.FindAsync(productId);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching product by ID: {ProductId}", productId);
                return await Task.FromResult<Product?>(null);
            }
        }

        public async Task<IEnumerable<Product>?> GetAllProducts(
            int pageNumber = 1,
            int pageSize = 10,
            string? category = null,
            string? name = null,
            float? minPrice = null,
            float? maxPrice = null)
        {
            this.logger.LogInformation("Fetching all products with filters");

            try
            {
                // Start with all products
                var query = this.dbContext.Products.AsQueryable();

                // Apply category filter
                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(p => p.Categories != null && p.Categories.Contains(category));
                }

                // Apply name filter
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Name.Contains(name));
                }

                // Apply price range filter
                if (minPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= maxPrice.Value);
                }

                // Apply OrderBy to ensure predictable results
                query = query.OrderBy(p => p.ProductID);

                // Ensure the page number is not negative
                pageNumber = Math.Max(pageNumber, 1);

                // Apply pagination
                var skip = (pageNumber - 1) * pageSize;
                var products = await query.Skip(skip).Take(pageSize).ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching products with filters");
                return await Task.FromResult<IEnumerable<Product>?>(null);
            }
        }

        public async Task<IEnumerable<Product>?> GetProductsByUserId(string userId)
        {
            this.logger.LogInformation("Fetching products by User ID: {UserId}", userId);
            try
            {
                return await this.dbContext.Products.Where(r => r.UserID == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching products by User ID: {UserId}", userId);
                return await Task.FromResult<IEnumerable<Product>?>(null);
            }
        }

        public async Task<Product?> CreateProduct(Product product, IFormFile file)
        {
            this.logger.LogInformation("Creating new product: {ProductName}", product.Name);
            try
            {
                product.SnapShotImageName = file.FileName;
                using (var stream = file.OpenReadStream())
                {
                    var imageUrl = await this.blobConnector.UploadImageAsync(product.ProductID.ToString(), product.SnapShotImageName, stream);
                    product.ImageUrl = imageUrl!;
                }

                this.dbContext.Products.Add(product);
                await this.dbContext.SaveChangesAsync();
                this.logger.LogInformation("Product created successfully: {ProductId}", product.ProductID);
                return await Task.FromResult<Product?>(product);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating product: {ProductName}", product.Name);
                return await Task.FromResult<Product?>(null);
            }
        }

        public async Task<Product?> UpdateProduct(Product product, IFormFile file)
        {
            this.logger.LogInformation("Updating product: {ProductId}", product.ProductID);
            try
            {
                product.SnapShotImageName = file.FileName;
                await this.blobConnector.DeleteImageAsync(product.ProductID.ToString(), product.Name);
                using (var stream = file.OpenReadStream())
                {
                    var imageUrl = await this.blobConnector.UploadImageAsync(product.ProductID.ToString(), product.SnapShotImageName, stream);
                    product.ImageUrl = imageUrl!;
                }

                this.dbContext.Products.Update(product);
                await this.dbContext.SaveChangesAsync();
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
                var product = await this.dbContext.Products.FindAsync(productId);
                if (product != null)
                {
                    await this.blobConnector.DeleteImageAsync(product.ProductID.ToString(), product.SnapShotImageName);
                    this.dbContext.Products.Remove(product);
                    await this.dbContext.SaveChangesAsync();
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
