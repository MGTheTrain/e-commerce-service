// <copyright file="ProductServiceTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.ApplicationTest.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Mgtt.ECom.Application.Services;
    using Mgtt.ECom.Domain.ProductManagement;
    using Mgtt.ECom.Infrastructure.Connectors;
    using Mgtt.ECom.Infrastructure.Settings;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class ProductServiceTests : IDisposable
    {
        private readonly PsqlDbContext dbContext;
        private readonly ProductService productService;

        public ProductServiceTests()
        {
            var options = new DbContextOptionsBuilder<PsqlDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new PsqlDbContext(options);

            var initialProducts = new List<Product>
            {
                new Product { UserID = Guid.NewGuid().ToString(), Name = "Product 1", SnapShotImageName = "snapshot.jpg", Price = 100, Categories = new List<string> { "Category1" } },
                new Product { UserID = Guid.NewGuid().ToString(), Name = "Product 2", SnapShotImageName = "snapshot.jpg", Price = 200, Categories = new List<string> { "Category2" } },
                new Product { UserID = Guid.NewGuid().ToString(), Name = "Product 3", SnapShotImageName = "snapshot.jpg", Price = 150, Categories = new List<string> { "Category1" } },
                new Product { UserID = Guid.NewGuid().ToString(), Name = "Product 4", SnapShotImageName = "snapshot.jpg", Price = 250, Categories = new List<string> { "Category2" } },
            };

            this.dbContext.Products.AddRange(initialProducts);
            this.dbContext.SaveChanges();

            var awsS3LoggerMock = Mock.Of<ILogger<AwsS3Connector>>();
            var awsS3Settings = new AwsS3Settings
            {
                UtilizeLocalStack = true,
            };

            var mockOptions = Options.Create(awsS3Settings);
            var awsS3Connector = new AwsS3Connector(awsS3LoggerMock, mockOptions);

            var loggerMock = new Mock<ILogger<ProductService>>();
            this.productService = new ProductService(this.dbContext, awsS3Connector, loggerMock.Object);
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }

        private IFormFile CreateTestFormFile(string fileName, string content)
        {
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            return new FormFile(stream, 0, stream.Length, "id_from_form", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain",
            };
        }

        [Fact]
        public async Task Test_CreateProduct_GetProductById()
        {
            // Arrange
            var product = new Product { UserID = Guid.NewGuid().ToString(), Name = "New Product", SnapShotImageName = "snapshot.jpg", Price = 300, Categories = new List<string> { "Category3" } };
            var formFile = this.CreateTestFormFile("test.txt", "This is a test file.");

            // Act
            await this.productService.CreateProduct(product, formFile);
            var result = await this.productService.GetProductById(product.ProductID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.UserID, result.UserID);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);
        }

        [Fact]
        public async Task Test_UpdateProduct()
        {
            // Arrange
            var product = new Product { UserID = Guid.NewGuid().ToString(), Name = "Product to Update", SnapShotImageName = "snapshot.jpg", Price = 400, Categories = new List<string> { "Category1" } };
            var formFile = this.CreateTestFormFile("test.txt", "This is a test file.");
            await this.productService.CreateProduct(product, formFile);

            // Act
            product.Name = "Updated Product";
            await this.productService.UpdateProduct(product, formFile);
            var updatedProduct = await this.productService.GetProductById(product.ProductID);

            // Assert
            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated Product", updatedProduct.Name);
        }

        [Fact]
        public async Task Test_DeleteProduct()
        {
            // Arrange
            var product = new Product { UserID = Guid.NewGuid().ToString(), Name = "Product to Delete", SnapShotImageName = "snapshot.jpg", Price = 500, Categories = new List<string> { "Category1" } };
            var formFile = this.CreateTestFormFile("test.txt", "This is a test file.");
            await this.productService.CreateProduct(product, formFile);

            // Act
            await this.productService.DeleteProduct(product.ProductID);
            var deletedProduct = await this.productService.GetProductById(product.ProductID);

            // Assert
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task Test_GetAllProducts()
        {
            // Act
            var products = await this.productService.GetAllProducts();

            // Assert
            Assert.NotNull(products);
            Assert.Equal(4, products.Count()); // Assuming there are 4 initial products in the in-memory context
        }

        [Fact]
        public async Task Test_GetAllProducts_WithPagination()
        {
            // Act
            var productsPage1 = await this.productService.GetAllProducts(pageNumber: 1, pageSize: 2);
            var productsPage2 = await this.productService.GetAllProducts(pageNumber: 2, pageSize: 2);

            // Assert
            Assert.NotNull(productsPage1);
            Assert.Equal(2, productsPage1.Count()); // Page 1 should have 2 products

            Assert.NotNull(productsPage2);
            Assert.Equal(2, productsPage2.Count()); // Page 2 should have 2 products
        }

        [Fact]
        public async Task Test_GetAllProducts_WithFiltering()
        {
            // Act
            var productsCategory1 = await this.productService.GetAllProducts(category: "Category1");
            var productsCategory2 = await this.productService.GetAllProducts(category: "Category2");
            var productsByName = await this.productService.GetAllProducts(name: "Product 1");
            var productsByPriceRange = await this.productService.GetAllProducts(minPrice: 100, maxPrice: 150);

            // Assert
            Assert.NotNull(productsCategory1);
            Assert.Equal(2, productsCategory1.Count()); // There are 2 products in Category1

            Assert.NotNull(productsCategory2);
            Assert.Equal(2, productsCategory2.Count()); // There are 2 products in Category2

            Assert.NotNull(productsByName);
            Assert.Single(productsByName); // Only 1 product with the name "Product 1"

            Assert.NotNull(productsByPriceRange);
            Assert.Equal(2, productsByPriceRange.Count()); // There are 2 products in the price range 100 to 150
        }
    }
}