using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace XavierPlayLandAPI.Filters.ExceptionFilters
{
    public class HandleUpdateExceptionsFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is UpdateProductException)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    message = "An error occurred while updating the product item.",
                    details = context.Exception.Message
                });
                context.ExceptionHandled = true;
            }
            else if (context.Exception is UpdateCategoryException)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    message = "An error occurred while updating the category item.",
                    details = context.Exception.Message
                });
                context.ExceptionHandled = true;
            }
            else if (context.Exception is UpdateUserException)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    message = "An error occurred while updating the user item.",
                    details = context.Exception.Message
                });
                context.ExceptionHandled = true;
            }

            base.OnException(context);
        }

        public class UpdateProductException : Exception
        {
            public UpdateProductException(string message) : base(message) { }
        }
        public class UpdateCategoryException : Exception
        {
            public UpdateCategoryException(string message) : base(message) { }
        }
        public class UpdateUserException : Exception
        {
            public UpdateUserException(string message) : base(message) { }
        }
    }
}
