// <copyright file="CategoryController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.ProductManagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.ProductManagement;
    using Mgtt.ECom.Web.V1.ProductManagement.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using static System.Collections.Specialized.BitVector32;

    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryDTO">The category data transfer object containing category details.</param>
        /// <returns>The newly created category.</returns>
        /// <response code="201">Returns the newly created category.</response>
        /// <response code="400">If the category data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategory(CategoryRequestDTO categoryDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var category = new Category
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
            };

            var action = await this.categoryService.CreateCategory(category);
            if (action == null)
            {
                return this.BadRequest();
            }

            var categoryResponseDTO = new CategoryResponseDTO
            {
                CategoryID = category.CategoryID,
                Name = category.Name,
                Description = category.Description,
            };

            return this.CreatedAtAction(nameof(this.CreateCategory), categoryResponseDTO);
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        /// <response code="200">Returns a list of all categories.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryResponseDTO>))]
        public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetAllCategories()
        {
            var categories = await this.categoryService.GetAllCategories();
            var categoryDTOs = new List<CategoryResponseDTO>();

            foreach (var category in categories)
            {
                categoryDTOs.Add(new CategoryResponseDTO
                {
                    CategoryID = category.CategoryID,
                    Name = category.Name,
                    Description = category.Description,
                });
            }

            return this.Ok(categoryDTOs);
        }

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>The category with the specified ID.</returns>
        /// <response code="200">Returns the category with the specified ID.</response>
        /// <response code="404">If the category is not found.</response>
        [HttpGet("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponseDTO>> GetCategoryById(Guid categoryId)
        {
            var category = await this.categoryService.GetCategoryById(categoryId);

            if (category == null)
            {
                return this.NotFound();
            }

            var categoryDTO = new CategoryResponseDTO
            {
                CategoryID = category.CategoryID,
                Name = category.Name,
                Description = category.Description,
            };

            return this.Ok(categoryDTO);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="categoryId">The ID of the category to update.</param>
        /// <param name="categoryDTO">The category data transfer object containing updated category details.</param>
        /// <returns>No content response if successful.</returns>
        /// <response code="204">If the category was successfully updated.</response>
        /// <response code="400">If the category data is invalid.</response>
        /// <response code="404">If the category is not found.</response>
        [HttpPut("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, CategoryRequestDTO categoryDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var category = await this.categoryService.GetCategoryById(categoryId);

            if (category == null)
            {
                return this.NotFound();
            }

            category.Name = categoryDTO.Name;
            category.Description = categoryDTO.Description;

            var action = await this.categoryService.UpdateCategory(category);
            if (action == null)
            {
                return this.BadRequest();
            }

            var categoryResponseDTO = new CategoryResponseDTO
            {
                CategoryID = category.CategoryID,
                Name = category.Name,
                Description = category.Description,
            };

            return this.Ok(categoryResponseDTO);
        }

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete.</param>
        /// <returns>No content response if successful.</returns>
        /// <response code="204">If the category was successfully deleted.</response>
        /// <response code="404">If the category is not found.</response>
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var existingCategory = await this.categoryService.GetCategoryById(categoryId);

            if (existingCategory == null)
            {
                return this.NotFound();
            }

            await this.categoryService.DeleteCategory(categoryId);

            return this.NoContent();
        }
    }
}