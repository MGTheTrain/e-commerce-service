using Microsoft.AspNetCore.Mvc;
using Mgtt.ECom.Web.v1.ProductManagement.DTOs;
using Mgtt.ECom.Domain.ProductManagement;
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

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductRequestDTO productDTO)
        {
            var product = new Product
            {
                CategoryID = productDTO.CategoryID,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Stock = productDTO.Stock,
                ImageUrl = productDTO.ImageUrl
            };

            await _productService.CreateProduct(product);

            return CreatedAtAction(nameof(GetProductById), new { productId = product.ProductID }, product);
        }

        [HttpGet]
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

        [HttpGet("{productId}")]
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

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(Guid productId, ProductRequestDTO productDTO)
        {
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

            await _productService.UpdateProduct(product);

            return NoContent();
        }

        [HttpDelete("{productId}")]
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
