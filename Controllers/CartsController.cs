using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using milestone3.Interfaces;
using milestone3.Models;

namespace milestone3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartsController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        // GET: api/Carts
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Cart>))]
        public IActionResult GetCart()
        {
            var carts = _cartRepository.GetCarts();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(carts);
        }

        // GET: api/Carts/CustomerId
        [HttpGet("{customerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Cart>))]
        public IActionResult GetCartByCustomer(int customerId)
        {
            var cart = _cartRepository.GetCartByCustomer(customerId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(cart);
        }
        // POST: api/Carts
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCart(Cart cartCreate)
        {
            if (cartCreate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_cartRepository.CreateCart(cartCreate))
            {
                ModelState.AddModelError("", "Something went wrong while creating cart");
                return StatusCode(500, ModelState);
            }

            return Created("/api/carts/" + cartCreate.Id, cartCreate);
        }

        // PUT: api/Carts/5
        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCart(int id, Cart cartUpdate)
        {
            if (cartUpdate == null)
                return BadRequest(ModelState);

            if (cartUpdate.Id != id)
                return BadRequest(ModelState);

            if (!_cartRepository.CartExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_cartRepository.UpdateCart(cartUpdate))
            {
                ModelState.AddModelError("", "something went wrong while updating cart");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
