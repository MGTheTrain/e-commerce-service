using Mgtt.ECom.Domain.ProductManagement;
using Mgtt.ECom.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly PsqlDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(PsqlDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Product> GetProductById(Guid productId)
        {
            _logger.LogInformation("Fetching product by ID: {ProductId}", productId);
            try
            {
                return await _context.Products.FindAsync(productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product by ID: {ProductId}", productId);
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            _logger.LogInformation("Fetching all products");
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all products");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(Guid categoryId)
        {
            _logger.LogInformation("Fetching products by Category ID: {CategoryId}", categoryId);
            try
            {
                return await _context.Products.Where(p => p.CategoryID == categoryId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products by Category ID: {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task CreateProduct(Product product)
        {
            _logger.LogInformation("Creating new product: {ProductName}", product.Name);
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Product created successfully: {ProductId}", product.ProductID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product: {ProductName}", product.Name);
                throw;
            }
        }

        public async Task UpdateProduct(Product product)
        {
            _logger.LogInformation("Updating product: {ProductId}", product.ProductID);
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Product updated successfully: {ProductId}", product.ProductID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product: {ProductId}", product.ProductID);
                throw;
            }
        }

        public async Task DeleteProduct(Guid productId)
        {
            _logger.LogInformation("Deleting product: {ProductId}", productId);
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Product deleted successfully: {ProductId}", productId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product: {ProductId}", productId);
                throw;
            }
        }
    }
}
