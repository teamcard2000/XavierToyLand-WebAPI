using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XavierPlayLandAPI.Models.Repositories;
using XavierPlayLandAPI.Models;
using XavierPlayLandAPI.Filters.ActionFilters;
using XavierPlayLandAPI.Filters.ExceptionFilters;
using Microsoft.AspNetCore.Authorization;

namespace XavierPlayLandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _productRepository.GetAllProducts());
        }

        [HttpGet("{id}")]
        [ValidateProductIdFilter]
        public async Task<IActionResult> GetProduct(int id)
        {
            return Ok(await _productRepository.GetProductById(id));
        }

        [HttpPost]
        [ValidateAddProductFilter]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (product.CategoryId.HasValue && !await _categoryRepository.CategoryExists(product.CategoryId.Value))
            {
                return BadRequest("The Category ID you are inputting does not exist!");
            }

            await _productRepository.AddProduct(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [ValidateProductIdFilter]
        [ValidateUpdateProductFilter]
        [HandleUpdateExceptionsFilter]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch!");
            }

            else if (product.CategoryId.HasValue && !await _categoryRepository.CategoryExists(product.CategoryId.Value)) 
            {
                return BadRequest("The Category ID you are inputting does not exist!");
            }

            await _productRepository.UpdateProduct(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ValidateProductIdFilter]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
            return NoContent();
        }
    }
}
