using System.Security.Cryptography;

namespace milestone3.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } = RandomNumberGenerator.GetInt32(1000);
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public ICollection<OrderItem> OrderItems { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
