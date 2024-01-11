using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XavierPlayLandAPI.Models.Repositories;
using XavierPlayLandAPI.Models;

namespace XavierPlayLandAPI.Filters.ActionFilters
{
    public class ValidateCategoryIdFilterAttribute : ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("id"))
            {
                context.Result = new BadRequestObjectResult("Category ID is missing.");
                return;
            }

            if (context.ActionArguments["id"] is not int id || id <= 0)
            {
                context.Result = new BadRequestObjectResult("Category ID is invalid.");
                return;
            }

            var categoryRepository = context.HttpContext.RequestServices.GetService<ICategoryRepository>();
            var category = await categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                context.Result = new NotFoundObjectResult($"Category with ID {id} not found.");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
