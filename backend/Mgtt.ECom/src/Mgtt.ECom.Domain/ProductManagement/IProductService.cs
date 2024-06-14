namespace Mgtt.ECom.Domain.ProductManagement;

public interface IProductService
{
    Task<Product?> GetProductById(Guid productId);
    Task<IEnumerable<Product>?> GetAllProducts();
    Task<IEnumerable<Product>?> GetProductsByCategory(Guid categoryId);
    Task<Product?> CreateProduct(Product product);
    Task<Product?> UpdateProduct(Product product);
    Task DeleteProduct(Guid productId);
}
