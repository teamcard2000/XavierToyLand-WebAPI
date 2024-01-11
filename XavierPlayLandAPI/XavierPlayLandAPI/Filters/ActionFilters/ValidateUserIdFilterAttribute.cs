using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XavierPlayLandAPI.Models.Repositories;

namespace XavierPlayLandAPI.Filters.ActionFilters
{
    public class ValidateUserIdFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("id"))
            {
                context.Result = new BadRequestObjectResult("User ID is missing.");
                return;
            }

            if (context.ActionArguments["id"] is not int id || id <= 0)
            {
                context.Result = new BadRequestObjectResult("User ID is invalid.");
                return;
            }

            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = userRepository.GetUserById(id);
            if (user == null)
            {
                context.Result = new NotFoundObjectResult($"User with ID {id} not found.");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
