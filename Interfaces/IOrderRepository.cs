using milestone3.Models;

namespace milestone3.Interfaces
{
    public interface IOrderRepository
    {
        ICollection<Order> GetOrders();
        bool CreateOrder(Order order);
        bool Save();
        
    }
}
