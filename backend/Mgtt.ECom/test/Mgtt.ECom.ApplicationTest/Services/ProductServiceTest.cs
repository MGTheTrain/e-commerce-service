// <copyright file="ProductServiceTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.ApplicationTest.Services
{
    using System;
    using System.Threading.Tasks;
    using Mgtt.ECom.Application.Services;
    using Mgtt.ECom.Domain.ProductManagement;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class ProductServiceTests : IDisposable
    {
        private readonly PsqlDbContext _dbContext;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            var options = new DbContextOptionsBuilder<PsqlDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            this._dbContext = new PsqlDbContext(options);

            var initialProducts = new List<Product>
            {
                new Product { Name = "Product 1", Price = 100 },
                new Product { Name = "Product 2", Price = 200 }
            };

            this._dbContext.Products.AddRange(initialProducts);
            this._dbContext.SaveChanges();

            var loggerMock = new Mock<ILogger<ProductService>>();

            this._productService = new ProductService(this._dbContext, loggerMock.Object);
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
        }

        [Fact]
        public async Task Test_CreateProduct_GetProductById()
        {
            // Arrange
            var product = new Product { Name = "New Product", Price = 300 };

            // Act
            await this._productService.CreateProduct(product);
            var result = await this._productService.GetProductById(product.ProductID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);
        }

        [Fact]
        public async Task Test_UpdateProduct()
        {
            // Arrange
            var product = new Product { Name = "Product to Update", Price = 400 };
            await this._productService.CreateProduct(product);

            // Act
            product.Name = "Updated Product";
            await this._productService.UpdateProduct(product);
            var updatedProduct = await this._productService.GetProductById(product.ProductID);

            // Assert
            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated Product", updatedProduct.Name);
        }

        [Fact]
        public async Task Test_DeleteProduct()
        {
            // Arrange
            var product = new Product { Name = "Product to Delete", Price = 500 };
            await this._productService.CreateProduct(product);

            // Act
            await this._productService.DeleteProduct(product.ProductID);
            var deletedProduct = await this._productService.GetProductById(product.ProductID);

            // Assert
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task Test_GetAllProducts()
        {
            // Act
            var products = await this._productService.GetAllProducts();

            // Assert
            Assert.NotNull(products);
            Assert.Equal(2, products.Count()); // Assuming there are 2 initial products in the in-memory context
        }
    }
}
