using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XavierPlayLandAPI.Models.Repositories;
using XavierPlayLandAPI.Models;

namespace XavierPlayLandAPI.Filters.ActionFilters
{
    public class ValidateUpdateUserFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("user") || context.ActionArguments["user"] is not User user)
            {
                context.Result = new BadRequestObjectResult("User data is missing or invalid.");
                return;
            }

            if (user.Id <= 0)
            {
                context.Result = new BadRequestObjectResult("Invalid user ID.");
                return;
            }

            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var existingUser = userRepository.GetUserById(user.Id);
            if (existingUser == null)
            {
                context.Result = new NotFoundObjectResult($"User with ID {user.Id} not found.");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
