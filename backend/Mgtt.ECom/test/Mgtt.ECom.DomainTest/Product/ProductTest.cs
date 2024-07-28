// <copyright file="ProductTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.DomainTest.Product
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Xunit;

    public class ProductTests
    {
        [Fact]
        public void Product_Validation_Succeeds_With_Valid_Data()
        {
            // Arrange
            var product = new Mgtt.ECom.Domain.ProductManagement.Product
            {
                UserID = Guid.NewGuid().ToString(),
                Categories = new List<string> { "Electronics", "Gadgets" },
                Name = "Test Product",
                SnapShotImageName = "snapshot.jpg",
                Description = "Product Description",
                Price = 99.99f,
                Stock = 10,
                ImageUrl = "https://example.com/product-image.jpg",
            };

            // Act
            var validationResults = this.ValidateProduct(product);

            // Assert
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("user-id-001", new string[] { "Electronics" }, "", "snaphot.jpg", "Product Description", 99.99f, 10, "https://example.com/product-image.jpg", "Name")]
        [InlineData("user-id-002", new string[] { "Electronics" }, "snaphot.jpg", "Test Product", "Product Description", 0.0f, 10, "https://example.com/product-image.jpg", "Price")]
        [InlineData("user-id-003", new string[] { "Electronics" }, "snaphot.jpg", "Test Product", "Product Description", 99.99f, -1, "https://example.com/product-image.jpg", "Stock")]
        [InlineData("user-id-004", new string[] { "Electronics" }, "snaphot.jpg", "Test Product", "Product Description", 99.99f, 10, "", "ImageUrl")]
        public void Product_Validation_Fails_With_Invalid_Data(string userID, string[] categories, string name, string snapShotImageName, string description, float price, int stock, string imageUrl, string expectedMemberName)
        {
            // Arrange
            var product = new Mgtt.ECom.Domain.ProductManagement.Product
            {
                UserID = userID,
                Categories = categories != null ? new List<string>(categories) : null,
                Name = name,
                SnapShotImageName = snapShotImageName,
                Description = description,
                Price = price,
                Stock = stock,
                ImageUrl = imageUrl,
            };

            // Act
            var validationResults = this.ValidateProduct(product);

            // Assert
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, vr => vr.MemberNames.Contains(expectedMemberName));
        }

        private IList<ValidationResult> ValidateProduct(Mgtt.ECom.Domain.ProductManagement.Product product)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(product, serviceProvider: null, items: null);
            Validator.TryValidateObject(product, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
