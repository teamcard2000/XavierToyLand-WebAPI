using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XavierPlayLandAPI.Models.Repositories;
using XavierPlayLandAPI.Models;

namespace XavierPlayLandAPI.Filters.ActionFilters
{
    public class ValidateAddEntityFilterAttribute : ActionFilterAttribute
    {
        private readonly EntityType _entityType;

        public ValidateAddEntityFilterAttribute(EntityType entityType)
        {
            _entityType = entityType;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            switch (_entityType)
            {
                case EntityType.Product:
                    await ValidateProduct(context);
                    break;
                case EntityType.Category:
                    await ValidateCategory(context);
                    break;
                case EntityType.User:
                    ValidateUser(context);
                    break;
                case EntityType.Order:
                    ValidateOrder(context);
                    break;
                case EntityType.Review:
                    await ValidateUserReview(context);
                    break;
                default:
                    context.Result = new BadRequestObjectResult("Invalid entity type.");
                    return;
            }

            base.OnActionExecuting(context);
        }

        private async Task ValidateProduct(ActionExecutingContext context)
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
        }

        private async Task ValidateCategory(ActionExecutingContext context)
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
        }

        private void ValidateUser(ActionExecutingContext context)
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
                var existingUser = userRepository?.GetUserById(user.Id);
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
        }

        private void ValidateOrder(ActionExecutingContext context)
        {
            var orderRepository = context.HttpContext.RequestServices.GetService<IOrderRepository>();
            var order = context.ActionArguments["order"] as Order;

            if (order == null)
            {
                context.ModelState.AddModelError("Order", "Order is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            else
            {
                var existingOrder = orderRepository?.GetOrderById(order.Id);
                if (existingOrder != null)
                {
                    context.ModelState.AddModelError("Order", "This order already exists.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                    return;
                }
            }
        }

        private async Task ValidateUserReview(ActionExecutingContext context)
        {
            var reviewRepository = context.HttpContext.RequestServices.GetService<IUserReviewRepository>();
            var review = context.ActionArguments["review"] as UserReview;

            if (review == null)
            {
                context.ModelState.AddModelError("Review", "User review is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            else
            {
                var existingReview = await reviewRepository.GetReviewById(review.Id);
                if (existingReview != null)
                {
                    context.ModelState.AddModelError("Review", "This user review already exists.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                    return;
                }
            }
        }
    }
}
