using milestone3.Data;
using milestone3.Interfaces;
using milestone3.Models;

namespace milestone3.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public Product GetProduct(int id)
        {
            return _context.Products.Where(p => p.Id == id).FirstOrDefault();
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.AsEnumerable();
        }

        public bool CreateProduct(Product product)
        {
            _context.Add(product);
            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            _context.Remove(product);
            return Save();
        }

        public bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public bool UpdateProduct(Product product)
        {
            _context.Update(product);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
