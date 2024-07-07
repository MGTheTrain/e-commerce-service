// <copyright file="PsqlDbContextTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.PersistenceTest.DataAccess
{
    using System;
    using System.Linq;
    using Mgtt.ECom.Domain.ProductManagement;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class PsqlDbContextTests
    {
        private DbContextOptions<PsqlDbContext> GetInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<PsqlDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
        }

        [Fact]
        public void Can_Create_Read_Update_Delete_Product()
        {
            // Arrange
            var options = this.GetInMemoryDbContextOptions();

            using (var context = new PsqlDbContext(options))
            {
                var product = new Product
                {
                    Categories = new System.Collections.Generic.List<string> { "Electronics", "Gadgets" },
                    Name = "Test Product",
                    Description = "Product Description",
                    Price = 99.99f,
                    Stock = 10,
                    ImageUrl = "https://example.com/product-image.jpg",
                };

                // Act
                // Create
                context.Products.Add(product);
                context.SaveChanges();

                // Read
                var retrievedProduct = context.Products.FirstOrDefault(p => p.Name == "Test Product");
                Assert.NotNull(retrievedProduct);
                Assert.Equal("Test Product", retrievedProduct.Name);
                Assert.Equal(99.99f, retrievedProduct.Price);
                Assert.Equal(10, retrievedProduct.Stock);
                Assert.Equal("https://example.com/product-image.jpg", retrievedProduct.ImageUrl);

                // Update
                retrievedProduct.Description = "Updated Product Description";
                context.SaveChanges();

                var updatedProduct = context.Products.FirstOrDefault(p => p.Name == "Test Product");
                Assert.NotNull(updatedProduct);
                Assert.Equal("Updated Product Description", updatedProduct.Description);

                // Delete
                context.Products.Remove(updatedProduct);
                context.SaveChanges();

                var deletedProduct = context.Products.FirstOrDefault(p => p.Name == "Test Product");
                Assert.Null(deletedProduct);
            }
        }
    }
}
