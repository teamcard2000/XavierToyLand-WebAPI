using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XavierPlayLandAPI.Models.Repositories;
using XavierPlayLandAPI.Models;

namespace XavierPlayLandAPI.Filters.ActionFilters
{
    public class ValidateAddCategoryFilterAttribute : ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var categoryRepository = context.HttpContext.RequestServices.GetService<ICategoryRepository>();
            var category = context.ActionArguments["category"] as Category;

            if (category == null)
            {
                context.ModelState.AddModelError("Category", "Category item is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            else
            {
                var existingCategory = await categoryRepository.GetCategoryById(category.Id);
                if (existingCategory != null)
                {
                    context.ModelState.AddModelError("Category", "This category already exists.");
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
