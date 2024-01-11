using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XavierPlayLandAPI.Models.Repositories;
using XavierPlayLandAPI.Models;

namespace XavierPlayLandAPI.Filters.ActionFilters
{
    public class ValidateAddProductFilterAttribute : ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var productRepository = context.HttpContext.RequestServices.GetService<IProductRepository>();
            var product = context.ActionArguments["product"] as Product;

            if (product == null)
            {
                context.ModelState.AddModelError("Product", "Product item is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            else
            {
                var existingProduct = await productRepository.GetProductById(product.Id);
                if (existingProduct != null)
                {
                    context.ModelState.AddModelError("Product", "A product item already exists.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                    return;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
