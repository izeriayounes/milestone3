using Microsoft.EntityFrameworkCore;
using milestone3.Data;
using milestone3.Interfaces;
using milestone3.Models;

namespace milestone3.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _context;

        public CustomerRepository(DataContext context)
        {
            _context = context;
        }
        public bool Create(Customer customer)
        {
            var HashedCustomer = new Customer
            {
                Username = customer.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(customer.Password)
            };

            _context.Add(HashedCustomer);
            return Save();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomer(int id)
        {
            return _context.Customers
                    .Include(c => c.Cart)
                    .FirstOrDefault(c => c.Id == id);
        }

        public Customer GetCustomerByUsername(string username)
        {
            return _context.Customers.Where(c => c.Username == username).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Customer customer)
        {
            _context.Update(customer);
            return Save();
        }

        public bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
