using Mgtt.ECom.Domain.ProductManagement;
using Mgtt.ECom.Persistence.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly PsqlDbContext _context;

        public CategoryService(PsqlDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryById(Guid categoryId)
        {
            return await Task.FromResult(_context.Categories.Find(categoryId));
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await Task.FromResult(_context.Categories.ToList());
        }

        public async Task CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(Guid categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
