namespace Mgtt.ECom.Domain.ProductManagement;

public interface ICategoryService
{
    Task<Category> GetCategoryById(Guid categoryId);
    Task<IEnumerable<Category>> GetAllCategories();
    Task CreateCategory(Category category);
    Task UpdateCategory(Category category);
    Task DeleteCategory(Guid categoryId);
}
