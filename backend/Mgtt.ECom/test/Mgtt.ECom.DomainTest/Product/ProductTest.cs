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
                Categories = new List<string> { "Electronics", "Gadgets" },
                Name = "Test Product",
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
        [InlineData("11111111-1111-1111-1111-111111111111", null, "", "Product Description", 99.99f, 10, "https://example.com/product-image.jpg", "Categories")]
        [InlineData("11111111-1111-1111-1111-111111111111", new string[] { "Electronics" }, "", "Product Description", 99.99f, 10, "https://example.com/product-image.jpg", "Name")]
        [InlineData("11111111-1111-1111-1111-111111111111", new string[] { "Electronics" }, "Test Product", "Product Description", 0.0f, 10, "https://example.com/product-image.jpg", "Price")]
        [InlineData("11111111-1111-1111-1111-111111111111", new string[] { "Electronics" }, "Test Product", "Product Description", 99.99f, -1, "https://example.com/product-image.jpg", "Stock")]
        [InlineData("11111111-1111-1111-1111-111111111111", new string[] { "Electronics" }, "Test Product", "Product Description", 99.99f, 10, "", "ImageUrl")]
        public void Product_Validation_Fails_With_Invalid_Data(string[] categories, string name, string description, float price, int stock, string imageUrl, string expectedMemberName)
        {
            // Arrange
            var product = new Mgtt.ECom.Domain.ProductManagement.Product
            {
                Categories = categories != null ? new List<string>(categories) : null,
                Name = name,
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
