using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using milestone3.DTO;
using milestone3.Interfaces;
using milestone3.Models;

namespace milestone3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartItemsController(ICartItemRepository cartItemRepository, IProductRepository productRepository, ICartRepository cartRepository, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        // GET: api/Items
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CartItem>))]
        public IActionResult GetItems()
        {
            var items = _cartItemRepository.GetItems();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(items);
        }

        // POST: api/CartItems
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateItem(int productId, int customerId)
        
        {
            if (productId <= 0)
                return BadRequest();

            var product = _productRepository.GetProduct(productId);
            var cart = _cartRepository.GetCartByCustomer(customerId);

            if (product == null)
            {   
                ModelState.AddModelError("", "Product or Cart not found");
                return NotFound();
            }

            var existingCartItem = _cartItemRepository.GetCartItemByProductId(productId, customerId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                if (!_cartItemRepository.Save())
                {
                    ModelState.AddModelError("", "Something went wrong while updating cart item");
                    return StatusCode(500, ModelState);
                }
                return NoContent();
            }

            var newItem = new CartItem
            {
                Product = product,
                Cart = cart,
                Quantity = 1
            };

            if (!_cartItemRepository.CreateItem(newItem))
            {
                ModelState.AddModelError("", "Something went wrong while creating cart item");
                return StatusCode(500, ModelState);
            }

            return Created($"/api/cartItems/{newItem.Id}", newItem);
        }

        // PUT: api/CartItems/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCartItem(int id, CartItemDTO itemUpdate)
        {
            if (id != itemUpdate.Id)
            {
                return BadRequest();
            }

            if (!_cartItemRepository.ItemExists(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = _cartItemRepository.GetItem(id);
            item.Quantity = itemUpdate.Quantity;

            _cartItemRepository.UpdateItem(item);

            return NoContent();
        }

        // DELETE: api/CartItems/5
        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteItem(int id)
        {
            if (!_cartItemRepository.ItemExists(id))
                return NotFound();

            var itemToDelete = _cartItemRepository.GetItem(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_cartItemRepository.DeleteItem(itemToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting item");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
