using milestone3.Models;

namespace milestone3.Interfaces
{
    public interface IOrderItemRepository
    {
        IEnumerable<OrderItem> GetItems();
        //OrderItem GetItem(int id);
        bool CreateItem(OrderItem cart);
        bool UpdateItem(OrderItem cart);
        bool DeleteItem(OrderItem cart);
        bool ItemExists(int id);
        bool Save();
    }
}
