using milestone3.Data;
using milestone3.Interfaces;
using milestone3.Models;

namespace milestone3.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateOrder(Order order)
        {
            _context.Add(order);
            return Save();
        }

        public ICollection<Order> GetOrders()
        {
            var orders = _context.Orders.ToList();
            return orders;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges() > 0;
            return saved;
        }
    }
}
