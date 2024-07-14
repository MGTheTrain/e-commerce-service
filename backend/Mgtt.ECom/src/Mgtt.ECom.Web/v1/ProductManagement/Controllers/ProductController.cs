// <copyright file="ProductController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.ProductManagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.ProductManagement;
    using Mgtt.ECom.Web.V1.ProductManagement.DTOs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// Determines if the user has the "manage:products" or "manage:own-product" permission, and if applicable, validates the user's products.
        /// </summary>
        /// <param name="isCreateOperation">Indicates whether the operation is a creation operation.</param>
        /// <param name="productId">The product id to check against.</param>
        /// <returns>True if the user has the required permissions and, if necessary, has valid products; otherwise, false.</returns>
        private async Task<string?> VerifyUserPermissionForProduct(bool isCreateOperation, Guid productId)
        {
            var permissionsClaims = this.User.FindAll("permissions");
            var userIdClaim = this.User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value;

            if (userId == null)
            {
                return null;
            }

            if (permissionsClaims.Any(x => x.Value.Split(' ').Contains("manage:products")))
            {
                if (!isCreateOperation && productId != Guid.Empty)
                {
                    var product = await this.productService.GetProductById(productId);
                    return product?.UserID ?? userId;
                }
                return userId;
            }

            if (permissionsClaims.Any(x => x.Value.Split(' ').Contains("manage:own-product")))
            {
                if (!isCreateOperation && productId != Guid.Empty)
                {
                    var userProducts = await this.productService.GetProductsByUserId(userId);
                    return userProducts?.Any(x => x.ProductID == productId) == true ? userId : null;
                }
                return userId;
            }

            return null;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productDTO">The product data transfer object containing product details.</param>
        /// <returns>The newly created product.</returns>
        /// <response code="201">Returns the newly created product.</response>
        /// <response code="400">If the product data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpPost]
        [Authorize("manage:products-and-own-product")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateProduct(ProductRequestDTO productDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = true;
            var userId = await this.VerifyUserPermissionForProduct(isCreateOperation, Guid.Empty);
            if (userId == null)
            {
                return this.Forbid();
            }

            var product = new Product
            {
                UserID = userId,
                Categories = productDTO.Categories,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Stock = productDTO.Stock,
                ImageUrl = productDTO.ImageUrl,
            };

            var action = await this.productService.CreateProduct(product);
            if (action == null)
            {
                return this.BadRequest();
            }

            var productResponseDTO = new ProductResponseDTO
            {
                ProductID = product.ProductID,
                UserID = product.UserID,
                Categories = product.Categories,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
            };

            return this.CreatedAtAction(nameof(this.CreateProduct), productResponseDTO);
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A list of all products.</returns>
        /// <response code="200">Returns a list of all products.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductResponseDTO>))]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetAllProducts()
        {
            var products = await this.productService.GetAllProducts();
            var productDTOs = new List<ProductResponseDTO>();

            foreach (var product in products)
            {
                productDTOs.Add(new ProductResponseDTO
                {
                    ProductID = product.ProductID,
                    UserID = product.UserID,
                    Categories = product.Categories,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    ImageUrl = product.ImageUrl,
                });
            }

            return this.Ok(productDTOs);
        }

        /// <summary>
        /// Retrieves products by user id.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of all products by user id.</returns>
        /// <response code="200">Returns a list of all products by user id.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductResponseDTO>))]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetProductsByUserId(string userId)
        {
            var products = await this.productService.GetProductsByUserId(userId);
            var productDTOs = new List<ProductResponseDTO>();

            foreach (var product in products)
            {
                productDTOs.Add(new ProductResponseDTO
                {
                    ProductID = product.ProductID,
                    UserID = product.UserID,
                    Categories = product.Categories,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    ImageUrl = product.ImageUrl,
                });
            }

            return this.Ok(productDTOs);
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        /// <response code="200">Returns the product with the specified ID.</response>
        /// <response code="404">If the product is not found.</response>
        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponseDTO>> GetProductById(Guid productId)
        {
            var product = await this.productService.GetProductById(productId);

            if (product == null)
            {
                return this.NotFound();
            }

            var productDTO = new ProductResponseDTO
            {
                ProductID = product.ProductID,
                UserID = product.UserID,
                Categories = product.Categories,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
            };

            return this.Ok(productDTO);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="productId">The ID of the product to update.</param>
        /// <param name="productDTO">The product data transfer object containing updated product details.</param>
        /// <returns>No content response if successful.</returns>
        /// <response code="204">If the product was successfully updated.</response>
        /// <response code="400">If the product data is invalid.</response>
        /// <response code="404">If the product is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpPut("{productId}")]
        [Authorize("manage:products-and-own-product")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateProduct(Guid productId, ProductRequestDTO productDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var isCreateOperation = false;
            var userId = await this.VerifyUserPermissionForProduct(isCreateOperation, productId);
            if (userId == null)
            {
                return this.Forbid();
            }

            var product = await this.productService.GetProductById(productId);

            if (product == null)
            {
                return this.NotFound();
            }

            product.Categories = productDTO.Categories;
            product.Name = productDTO.Name;
            product.Description = productDTO.Description;
            product.Price = productDTO.Price;
            product.Stock = productDTO.Stock;
            product.ImageUrl = productDTO.ImageUrl;

            var action = await this.productService.UpdateProduct(product);
            if (action == null)
            {
                return this.BadRequest();
            }

            var productResponseDTO = new ProductResponseDTO
            {
                ProductID = product.ProductID,
                UserID = product.UserID,
                Categories = product.Categories,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
            };

            return this.Ok(productResponseDTO);
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="productId">The ID of the product to delete.</param>
        /// <returns>No content response if successful.</returns>
        /// <response code="204">If the product was successfully deleted.</response>
        /// <response code="404">If the product is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not allowed to manage the resource.</response>
        [HttpDelete("{productId}")]
        [Authorize("manage:products-and-own-product")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var product = await this.productService.GetProductById(productId);

            if (product == null)
            {
                return this.NotFound();
            }

            var isCreateOperation = false;
            var userId = await this.VerifyUserPermissionForProduct(isCreateOperation, productId);
            if (userId == null)
            {
                return this.Forbid();
            }

            await this.productService.DeleteProduct(productId);

            return this.NoContent();
        }
    }
}