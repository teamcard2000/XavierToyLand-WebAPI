using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XavierPlayLandAPI.Models.Repositories;

namespace XavierPlayLandAPI.Filters.ActionFilters
{
    public class ValidateProductIdFilterAttribute : ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("id"))
            {
                context.Result = new BadRequestObjectResult("Product ID is missing.");
                return;
            }

            if (context.ActionArguments["id"] is not int id || id <= 0)
            {
                context.Result = new BadRequestObjectResult("Product ID is invalid.");
                return;
            }

            var productRepository = context.HttpContext.RequestServices.GetService<IProductRepository>();
            var product = await productRepository.GetProductById(id);
            if (product == null)
            {
                context.Result = new NotFoundObjectResult($"Product item with ID {id} not found.");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
