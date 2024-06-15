using Microsoft.AspNetCore.Mvc;
using Mgtt.ECom.Domain.ProductManagement;
using Mgtt.ECom.Web.v1.ProductManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mgtt.ECom.Web.v1.ProductManagement.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productDTO">The product data transfer object containing product details.</param>
        /// <returns>The newly created product.</returns>
        /// <response code="201">Returns the newly created product.</response>
        /// <response code="400">If the product data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct(ProductRequestDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                CategoryID = productDTO.CategoryID,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Stock = productDTO.Stock,
                ImageUrl = productDTO.ImageUrl
            };

            var action = await _productService.CreateProduct(product);
            if (action == null)
            {
                return BadRequest();
            }

            var productResponseDTO = new ProductResponseDTO
            {
                ProductID = product.ProductID,
                CategoryID = product.CategoryID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl
            };

            return CreatedAtAction(nameof(CreateProduct), productResponseDTO);
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
            var products = await _productService.GetAllProducts();
            var productDTOs = new List<ProductResponseDTO>();

            foreach (var product in products)
            {
                productDTOs.Add(new ProductResponseDTO
                {
                    ProductID = product.ProductID,
                    CategoryID = product.CategoryID,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    ImageUrl = product.ImageUrl
                });
            }

            return Ok(productDTOs);
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
            var product = await _productService.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            var productDTO = new ProductResponseDTO
            {
                ProductID = product.ProductID,
                CategoryID = product.CategoryID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl
            };

            return Ok(productDTO);
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
        [HttpPut("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(Guid productId, ProductRequestDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productService.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            product.CategoryID = productDTO.CategoryID;
            product.Name = productDTO.Name;
            product.Description = productDTO.Description;
            product.Price = productDTO.Price;
            product.Stock = productDTO.Stock;
            product.ImageUrl = productDTO.ImageUrl;

            var action = await _productService.UpdateProduct(product);
            if (action == null)
            {
                return BadRequest();
            }

            var productResponseDTO = new ProductResponseDTO
            {
                ProductID = product.ProductID,
                CategoryID = product.CategoryID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl
            };

            return Ok(productResponseDTO);
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="productId">The ID of the product to delete.</param>
        /// <returns>No content response if successful.</returns>
        /// <response code="204">If the product was successfully deleted.</response>
        /// <response code="404">If the product is not found.</response>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var product = await _productService.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProduct(productId);

            return NoContent();
        }
    }
}