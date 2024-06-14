using Mgtt.ECom.Domain.ProductManagement;
using Mgtt.ECom.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly PsqlDbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(PsqlDbContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Category?> GetCategoryById(Guid categoryId)
        {
            _logger.LogInformation("Fetching category by ID: {CategoryId}", categoryId);
            try
            {
                return await _context.Categories.FindAsync(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching category by ID: {CategoryId}", categoryId);
                return await Task.FromResult<Category?>(null);
            }
        }

        public async Task<IEnumerable<Category>?> GetAllCategories()
        {
            _logger.LogInformation("Fetching all categories");
            try
            {
                return await _context.Categories.Where(x => true).ToListAsync(); // opt for pagination for filtered list
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all categories");
                return await Task.FromResult<IEnumerable<Category>?>(null);
            }
        }

        public async Task<Category?> CreateCategory(Category category)
        {
            _logger.LogInformation("Creating new category: {CategoryId}", category.CategoryID);
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Category created successfully: {CategoryId}", category.CategoryID);
                return await Task.FromResult<Category?>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category: {CategoryId}", category.CategoryID);
                return await Task.FromResult<Category?>(null);
            }
        }

        public async Task<Category?> UpdateCategory(Category category)
        {
            _logger.LogInformation("Updating category: {CategoryId}", category.CategoryID);
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Category updated successfully: {CategoryId}", category.CategoryID);
                return await Task.FromResult<Category?>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category: {CategoryId}", category.CategoryID);
                return await Task.FromResult<Category?>(null);
            }
        }

        public async Task DeleteCategory(Guid categoryId)
        {
            _logger.LogInformation("Deleting category: {CategoryId}", categoryId);
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Category deleted successfully: {CategoryId}", categoryId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category: {CategoryId}", categoryId);
            }
        }
    }
}
