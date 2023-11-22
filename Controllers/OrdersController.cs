using Microsoft.AspNetCore.Mvc;
using milestone3.Interfaces;
using milestone3.Models;
using milestone3.Repository;

namespace milestone3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemsRepository;

        public OrdersController(IOrderRepository orderRepository, ICartItemRepository cartItemsRepository)
        {
            _orderRepository = orderRepository;
            _cartItemsRepository = cartItemsRepository;
        }

        // GET: api/Orders
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        public IActionResult GetOrders()
        {
            var orders = _orderRepository.GetOrders();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(orders);
        }

        // POST: api/Orders/Checkout
        [HttpPost("Checkout")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Checkout()
        {
            var cartItems = _cartItemsRepository.GetItems();

            if (cartItems == null)
                return BadRequest("No orders in the cart.");

            var orderItems = new List<OrderItem>();
            foreach (var cartItem in cartItems)
            {
                orderItems.Add(new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity
                });
            }

            var newOrder = new Order
            {
                CustomerId = 123, // Replace with actual customer ID
                TotalAmount = CalculateTotalAmount(orderItems), // Define the calculation logic
                OrderDate = DateTime.UtcNow, // Current date and time
                OrderItems = orderItems
            };

            if (!_orderRepository.CreateOrder(newOrder))
                return StatusCode(500, "Failed to create the order.");

            // Clear the cart after successful checkout
            foreach (var cartItem in cartItems)
            {
                _cartItemsRepository.DeleteItem(cartItem);
            }

            return Ok("Order created successfully.");
        }

        // Helper method to calculate the total amount for the order
        private decimal CalculateTotalAmount(List<OrderItem> orderItems)
        {
            decimal totalAmount = 0;
            foreach (var item in orderItems)
            {
                // Add logic to calculate the total amount for each order item
                // For example: totalAmount += item.Quantity * GetProductPriceById(item.ProductId);
            }
            return totalAmount;
        }
    }
}
