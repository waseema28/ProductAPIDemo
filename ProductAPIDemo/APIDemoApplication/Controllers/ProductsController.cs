using BusniessLogic.Interface;
using DataAccess.DatabaseObject;
using Microsoft.AspNetCore.Mvc;

namespace APIDemoApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10)
        {
            var products = await _productRepository.GetProductsAsync(page, pageSize);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Products product)
        {
            if (product == null)
                return BadRequest("Product data is null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            product.CreatedDate = DateTime.Now;
            var createdProduct = await _productRepository.CreateProductAsync(product);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductId }, createdProduct);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Products product)
        {
            if (product == null || id != product.ProductId)
                return BadRequest("Invalid data.");

            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
                return NotFound();

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            existingProduct.ModifiedDate = DateTime.Now;

            var updatedProduct = await _productRepository.UpdateProductAsync(existingProduct);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            var deleted = await _productRepository.DeleteProductAsync(id);
            if (!deleted)
                return StatusCode(500, "Could not delete the product.");

            return NoContent();
        }
    }
}
