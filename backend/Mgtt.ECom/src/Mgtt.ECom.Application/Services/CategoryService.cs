// <copyright file="CategoryService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.ProductManagement;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class CategoryService : ICategoryService
    {
        private readonly PsqlDbContext context;
        private readonly ILogger<CategoryService> logger;

        public CategoryService(PsqlDbContext context, ILogger<CategoryService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<Category?> GetCategoryById(Guid categoryId)
        {
            this.logger.LogInformation("Fetching category by ID: {CategoryId}", categoryId);
            try
            {
                return await this.context.Categories.FindAsync(categoryId);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching category by ID: {CategoryId}", categoryId);
                return await Task.FromResult<Category?>(null);
            }
        }

        public async Task<IEnumerable<Category>?> GetAllCategories()
        {
            this.logger.LogInformation("Fetching all categories");
            try
            {
                return await this.context.Categories.Where(x => true).ToListAsync(); // opt for pagination for filtered list
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching all categories");
                return await Task.FromResult<IEnumerable<Category>?>(null);
            }
        }

        public async Task<Category?> CreateCategory(Category category)
        {
            this.logger.LogInformation("Creating new category: {CategoryId}", category.CategoryID);
            try
            {
                this.context.Categories.Add(category);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Category created successfully: {CategoryId}", category.CategoryID);
                return await Task.FromResult<Category?>(category);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating category: {CategoryId}", category.CategoryID);
                return await Task.FromResult<Category?>(null);
            }
        }

        public async Task<Category?> UpdateCategory(Category category)
        {
            this.logger.LogInformation("Updating category: {CategoryId}", category.CategoryID);
            try
            {
                this.context.Categories.Update(category);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Category updated successfully: {CategoryId}", category.CategoryID);
                return await Task.FromResult<Category?>(category);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating category: {CategoryId}", category.CategoryID);
                return await Task.FromResult<Category?>(null);
            }
        }

        public async Task DeleteCategory(Guid categoryId)
        {
            this.logger.LogInformation("Deleting category: {CategoryId}", categoryId);
            try
            {
                var category = await this.context.Categories.FindAsync(categoryId);
                if (category != null)
                {
                    this.context.Categories.Remove(category);
                    await this.context.SaveChangesAsync();
                    this.logger.LogInformation("Category deleted successfully: {CategoryId}", categoryId);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting category: {CategoryId}", categoryId);
            }
        }
    }
}
