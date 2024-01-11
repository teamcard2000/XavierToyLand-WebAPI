using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XavierPlayLandAPI.Models.Repositories;
using XavierPlayLandAPI.Models;

namespace XavierPlayLandAPI.Filters.ActionFilters
{
    public class ValidateUpdateCategoryFilterAttribute : ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("category") || context.ActionArguments["category"] is not Category category)
            {
                context.Result = new BadRequestObjectResult("Category data is missing or invalid.");
                return;
            }

            if (category.Id <= 0)
            {
                context.Result = new BadRequestObjectResult("Invalid category ID.");
                return;
            }

            var categoryRepository = context.HttpContext.RequestServices.GetService<ICategoryRepository>();
            var existingCategory = await categoryRepository.GetCategoryById(category.Id);
            if (existingCategory == null)
            {
                context.Result = new NotFoundObjectResult($"Category with ID {category.Id} not found.");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
