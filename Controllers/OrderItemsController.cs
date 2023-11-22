using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using milestone3.Interfaces;
using milestone3.Models;

namespace milestone3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemsController(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        // GET: api/Items
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrderItem>))]
        public IActionResult GetItems()
        {
            var items = _orderItemRepository.GetItems();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(items);
        }

        // POST: api/Items
        
    }
}
