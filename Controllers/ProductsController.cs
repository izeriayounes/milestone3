using Microsoft.AspNetCore.Mvc;
using milestone3.DTO;
using milestone3.Interfaces;
using milestone3.Models;
using System;

namespace milestone3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/Products
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetProduct(int id)
        {
            if (!_productRepository.ProductExists(id))
                return NotFound();

            var product = _productRepository.GetProduct(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(product);
        }

        // POST: api/Products
        //[HttpPost]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400)]
        //public IActionResult CreateProduct(Product productCreate)
        //{
        //    if (productCreate == null)
        //        return BadRequest();

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    if (!_productRepository.CreateProduct(productCreate))
        //    {
        //        ModelState.AddModelError("", "Something went wrong while creating product");
        //        return StatusCode(500, ModelState);
        //    }

        //    return Created("/api/products/" + productCreate.Id, productCreate);

        //}

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct( Product productCreate)
        {
            if (productCreate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //byte[]? imageBytes = null;
            //if (productCreate.Image != null && productCreate.Image.Length > 0)
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        productCreate.Image.CopyTo(memoryStream);
            //        imageBytes = memoryStream.ToArray();
            //    }
            //}

            //var product = new Product
            //{
            //    Name = productCreate.Name,
            //    Description = productCreate.Description,
            //    Price = productCreate.Price,
            //    Category = productCreate.Category,
            //    Image = imageBytes // Store the image as a byte array in the database
            //};

            if (!_productRepository.CreateProduct(productCreate))
            {
                ModelState.AddModelError("", "Something went wrong while creating product");
                return StatusCode(500, ModelState);
            }

            return Created("/api/products/" + productCreate.Id, productCreate);
        }


        // PUT: api/Products/5
        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int id, Product productUpdate)
        {
            if (productUpdate == null)
                return BadRequest(ModelState);

            if (productUpdate.Id != id)
                return BadRequest(ModelState);

            if (!_productRepository.ProductExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_productRepository.UpdateProduct(productUpdate))
            {
                ModelState.AddModelError("", "something went wrong while updating product");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(int id)
        {
            if (!_productRepository.ProductExists(id))
                return NotFound();

            var productToDelete = _productRepository.GetProduct(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_productRepository.DeleteProduct(productToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting product");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
