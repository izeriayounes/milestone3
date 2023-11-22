using milestone3.Models;

namespace milestone3.Interfaces
{
    public interface ICartRepository
    {
        ICollection<Cart> GetCarts();
        public Cart GetCartByCustomer(int customerId);
        public ICollection<CartItem> GetCartItemsByCustomer(int customerId);
        bool CreateCart(Cart cart);
        bool UpdateCart(Cart cart);
        bool DeleteCart(Cart cart);
        bool CartExists(int id);
        bool Save();
    }
}
