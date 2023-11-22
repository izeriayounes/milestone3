namespace milestone3.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Cart Cart { get; set; }

    }
}
