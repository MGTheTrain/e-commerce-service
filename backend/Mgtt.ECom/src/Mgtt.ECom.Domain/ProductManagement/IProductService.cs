namespace Mgtt.ECom.Domain.ProductManagement;

public interface IProductService
{
    Task<Product> GetProductById(Guid productId);
    Task<IEnumerable<Product>> GetAllProducts();
    Task<IEnumerable<Product>> GetProductsByCategory(Guid categoryId);
    Task CreateProduct(Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(Guid productId);
}
