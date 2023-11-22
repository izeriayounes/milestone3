using milestone3.Models;

namespace milestone3.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomer(int id);
        Customer GetCustomerByUsername(string username);
        bool Create(Customer customer);
        bool Update(Customer customer);
        bool CustomerExists(int id);
        bool Save();

    }
}
