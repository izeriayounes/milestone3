using milestone3.Data;
using milestone3.Interfaces;
using milestone3.Models;

namespace milestone3.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly DataContext _context;
        public OrderItemRepository(DataContext context)
        {
            _context = context;
        }

        public OrderItem GetItem(int id)
        {
            return _context.OrderItems.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool ItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.Id == id);
        }

        public bool CreateItem(OrderItem item)
        {
            _context.Add(item);
            return Save();
        }

        public bool DeleteItem(OrderItem item)
        {
            _context.Remove(item);
            return Save();
        }

        public IEnumerable<OrderItem> GetItems()
        {
            return _context.OrderItems.AsEnumerable();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateItem(OrderItem item)
        {
            _context.Update(item);
            return Save();
        }
    }
}
