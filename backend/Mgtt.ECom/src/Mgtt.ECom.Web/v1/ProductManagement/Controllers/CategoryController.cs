using Microsoft.AspNetCore.Mvc;
using Mgtt.ECom.Domain.ProductManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mgtt.ECom.Web.v1.ProductManagement.DTOs;

namespace Mgtt.ECom.Web.v1.ProductManagement.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryRequestDTO categoryDTO)
        {
            var category = new Category
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description
            };

            await _categoryService.CreateCategory(category);

            var categoryResponseDTO = new CategoryResponseDTO
            {
                CategoryID = category.CategoryID,
                Name = category.Name,
                Description = category.Description
            };

            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = categoryResponseDTO.CategoryID }, categoryResponseDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            var categoryDTOs = new List<CategoryResponseDTO>();

            foreach (var category in categories)
            {
                categoryDTOs.Add(new CategoryResponseDTO
                {
                    CategoryID = category.CategoryID,
                    Name = category.Name,
                    Description = category.Description
                });
            }

            return Ok(categoryDTOs);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryResponseDTO>> GetCategoryById(Guid categoryId)
        {
            var category = await _categoryService.GetCategoryById(categoryId);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDTO = new CategoryResponseDTO
            {
                CategoryID = category.CategoryID,
                Name = category.Name,
                Description = category.Description
            };

            return Ok(categoryDTO);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, CategoryRequestDTO categoryDTO)
        {
            var existingCategory = await _categoryService.GetCategoryById(categoryId);

            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = categoryDTO.Name;
            existingCategory.Description = categoryDTO.Description;

            await _categoryService.UpdateCategory(existingCategory);

            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var existingCategory = await _categoryService.GetCategoryById(categoryId);

            if (existingCategory == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteCategory(categoryId);

            return NoContent();
        }
    }
}
