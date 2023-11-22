using milestone3.Models;

namespace milestone3.Interfaces
{
    public interface ICartItemRepository
    {
        IEnumerable<CartItem> GetItems();
        CartItem GetItem(int id);
        CartItem GetCartItemByProductId(int productId, int customerId);
        bool CreateItem(CartItem cart);
        bool UpdateItem(CartItem cart);
        bool DeleteItem(CartItem cart);
        bool ItemExists(int id);
        bool Save();
    }
}
