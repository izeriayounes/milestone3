using Microsoft.EntityFrameworkCore;
using milestone3.Data;
using milestone3.Interfaces;
using milestone3.Models;

namespace milestone3.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;
        public CartRepository(DataContext context)
        {
            _context = context;
        }
        public bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }

        public bool CreateCart(Cart cart)
        {
            cart.Customer.Password = BCrypt.Net.BCrypt.HashPassword(cart.Customer.Password);
            _context.Add(cart);
            return Save();
        }

        public Cart GetCartByCustomer(int customerId)
        {
            return _context.Carts.Where(c => c.Customer.Id == customerId).FirstOrDefault();
        }
        public ICollection<CartItem> GetCartItemsByCustomer(int customerId)
        {
            var cart = _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault(c => c.Customer.Id == customerId);
            return cart?.CartItems;
        }

        public ICollection<Cart> GetCarts()
        {
            return _context.Carts.Include(c => c.CartItems).ToList();
        }

        public bool DeleteCart(Cart cart)
        {
            _context.Remove(cart);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateCart(Cart cart)
        {
            _context.Update(cart);
            return Save();
        }
    }
}
