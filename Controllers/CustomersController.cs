using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using milestone3.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using milestone3.Models;
using AutoMapper;
using milestone3.DTO;
using System.Collections.Generic;

namespace milestone3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository customerRepository, ICartRepository cartRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        // GET: api/Customers
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
        public IActionResult GetCustomers()
        {
            var customers = _customerRepository.GetCustomers();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(customers);
        }

        // GET api/Customers/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomer(int id)
        {
            if (!_customerRepository.CustomerExists(id))
                return NotFound();

            var customer = _customerRepository.GetCustomer(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(customer);
        }

        // GET api/Customers/5/cart-items
        [HttpGet("{id}/cart-items")]
        [ProducesResponseType(200, Type = typeof(ICollection<CartItemDTO>))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerCartItems(int id)
        {
            var customer = _customerRepository.GetCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }

            var cartItems = _cartRepository.GetCartItemsByCustomer(id);

            if (cartItems == null)
            {
                return NotFound();
            }

            return Ok(cartItems);
        }


        // POST api/Customers
        [HttpPost("register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCustomer(CustomerDTO customerCreate)
        {
            if (customerCreate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = _mapper.Map<Customer>(customerCreate);

            var cart = new Cart()
            {
                Customer = customer
            };

            if (!_cartRepository.CreateCart(cart))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Created("/api/customers/" + customer.Id, customer);
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Login(CustomerDTO customerLogin)
        {
            if (customerLogin == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = _customerRepository.GetCustomerByUsername(customerLogin.Username);

            if (customer == null)
                return BadRequest(new { message = "invalid credentials" });

            if (!BCrypt.Net.BCrypt.Verify(customerLogin.Password, customer.Password))
                return BadRequest(new { message = "invalid credentials" });

            var token = GenerateToken(customer.Username, customer.Id);

            return Ok(new { token, customerId = customer.Id, Message = "success" });
        }


        [Authorize]
        [HttpGet("validate-token")]
        public IActionResult ValidateToken()
        {
            var customerId = GetCurrentCustomerId(); 

            if (customerId == null)
            {
                return BadRequest(new { message = "Customer ID not found" });
            }

            return Ok(new { message = "Token is valid", customerId });
        }

        private string GetCurrentCustomerId()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var customerId = claimsIdentity?.FindFirst("customerId")?.Value;
            return customerId;
        }

        static private string GenerateToken(string username, int customerId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("this is my custom Secret key for authentication");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, username),
            new Claim("customerId", customerId.ToString())
        }),
                Expires = DateTime.UtcNow.AddDays(1), // Set expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
