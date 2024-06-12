namespace Mgtt.ECom.Domain.ProductManagement;

public interface IProductService
{
    Task<Product> GetProductById(int productId);
    Task<IEnumerable<Product>> GetAllProducts();
    Task<IEnumerable<Product>> GetProductsByCategory(int categoryId);
    Task CreateProduct(Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(int productId);
}
