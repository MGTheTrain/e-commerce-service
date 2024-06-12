using Mgtt.ECom.Domain.ProductManagement;
using Mgtt.ECom.Persistence.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly PsqlDbContext _context;

        public ProductService(PsqlDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductById(Guid productId)
        {
            return await Task.FromResult(_context.Products.Find(productId));
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await Task.FromResult(_context.Products.ToList());
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(Guid categoryId)
        {
            return await Task.FromResult(_context.Products.Where(p => p.CategoryID == categoryId).ToList());
        }

        public async Task CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
