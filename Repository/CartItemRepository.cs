using Microsoft.EntityFrameworkCore;
using milestone3.Data;
using milestone3.Interfaces;
using milestone3.Models;

namespace milestone3.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly DataContext _context;
        public CartItemRepository(DataContext context)
        {
            _context = context;
        }

        public CartItem GetItem(int id)
        {
            return _context.CartItems.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool ItemExists(int id)
        {
            return _context.CartItems.Any(e => e.Id == id);
        }

        public CartItem GetCartItemByProductId(int productId, int customerId)
        {
            return _context.CartItems
                    .Where(ci => ci.Product.Id == productId && ci.Cart.Customer.Id == customerId)
                    .FirstOrDefault();
        }

        public bool CreateItem(CartItem item)
        {
            _context.Add(item);
            return Save();
        }

        public bool DeleteItem(CartItem item)
        {
            _context.Remove(item);
            return Save();
        }

        public IEnumerable<CartItem> GetItems()
        {
            return _context.CartItems.Include(c => c.Product).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateItem(CartItem item)
        {
            _context.Update(item);
            return Save();
        }
    }
}
