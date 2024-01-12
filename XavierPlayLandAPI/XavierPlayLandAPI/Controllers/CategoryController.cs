using Microsoft.AspNetCore.Mvc;
using XavierPlayLandAPI.Models;
using XavierPlayLandAPI.Models.Repositories;
using System.Threading.Tasks;
using System.Linq;
using XavierPlayLandAPI.Filters.ActionFilters;
using XavierPlayLandAPI.Filters.ExceptionFilters;
using Microsoft.AspNetCore.Authorization;
using XavierPlayLandAPI.Filters;

namespace XavierPlayLandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoriesController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [ValidateEntityIdFilter(EntityType.Category)]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        [ValidateAddEntityFilter(EntityType.Category)]
        public async Task<IActionResult> AddCategory(Category category)
        {
            await _categoryRepository.AddCategory(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        [ValidateEntityIdFilter(EntityType.Category)]
        [ValidateUpdateEntityFilter(EntityType.Category)]
        [HandleUpdateExceptionsFilter]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest("Category ID mismatch!");
            }

            await _categoryRepository.UpdateCategory(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ValidateEntityIdFilter(EntityType.Category)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            // Check if any products are associated with this category
            if (await _productRepository.AnyProductWithCategoryId(id))
            {
                return BadRequest("Cannot delete category because it is associated with one or more products.");
            }

            await _categoryRepository.DeleteCategory(id);
            return NoContent();
        }
    }
}
