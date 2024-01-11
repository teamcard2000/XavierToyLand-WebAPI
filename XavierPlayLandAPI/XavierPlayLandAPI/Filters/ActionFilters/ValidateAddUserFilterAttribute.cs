using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XavierPlayLandAPI.Models.Repositories;
using XavierPlayLandAPI.Models;

namespace XavierPlayLandAPI.Filters.ActionFilters
{
    public class ValidateAddUserFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = context.ActionArguments["user"] as User;

            if (user == null)
            {
                context.ModelState.AddModelError("User", "User is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            else
            {
                var existingUser = userRepository.GetUserById(user.Id);
                if (existingUser != null)
                {
                    context.ModelState.AddModelError("User", "This user already exists.");
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
