using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XavierPlayLandAPI.Models.Repositories;
using XavierPlayLandAPI.Models;

namespace XavierPlayLandAPI.Filters.ActionFilters
{
    public class ValidateUpdateProductFilterAttribute : ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("product") || context.ActionArguments["product"] is not Product product)
            {
                context.Result = new BadRequestObjectResult("Product data is missing or invalid.");
                return;
            }

            if (product.Id <= 0)
            {
                context.Result = new BadRequestObjectResult("Invalid product ID.");
                return;
            }

            var productRepository = context.HttpContext.RequestServices.GetService<IProductRepository>();
            var existingProduct = await productRepository.GetProductById(product.Id);
            if (existingProduct == null)
            {
                context.Result = new NotFoundObjectResult($"Product item with ID {product.Id} not found.");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
