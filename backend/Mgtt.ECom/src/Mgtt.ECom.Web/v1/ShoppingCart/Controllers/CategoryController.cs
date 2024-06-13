using Microsoft.AspNetCore.Mvc;
using Mgtt.ECom.Web.v1.ProductManagement.DTOs;
using Mgtt.ECom.Domain.ProductManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = category.CategoryID }, category);
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
            var category = await _categoryService.GetCategoryById(categoryId);

            if (category ==
