using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using XavierPlayLandAPI.Models.Repositories;

namespace XavierPlayLandAPI.Filters.ActionFilters
{
    public class ValidateEntityIdFilterAttribute : ActionFilterAttribute
    {
        private readonly EntityType _entityType;

        public ValidateEntityIdFilterAttribute(EntityType entityType)
        {
            _entityType = entityType;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("id"))
            {
                context.Result = new BadRequestObjectResult($"{_entityType} ID is missing.");
                return;
            }

            if (context.ActionArguments["id"] is not int id || id <= 0)
            {
                context.Result = new BadRequestObjectResult($"{_entityType} ID is invalid.");
                return;
            }

            switch (_entityType)
            {
                case EntityType.Product:
                    await ValidateProduct(context, id);
                    break;
                case EntityType.Category:
                    await ValidateCategory(context, id);
                    break;
                case EntityType.User:
                    ValidateUser(context, id);
                    break;
                case EntityType.Order:
                    ValidateOrder(context, id);
                    break;
                case EntityType.Review:
                    await ValidateUserReview(context, id);
                    break;
                default:
                    context.Result = new BadRequestObjectResult("Invalid entity type.");
                    return;
            }

            base.OnActionExecuting(context);
        }

        private async Task ValidateProduct(ActionExecutingContext context, int id)
        {
            var productRepository = context.HttpContext.RequestServices.GetService<IProductRepository>();
            var product = await productRepository.GetProductById(id);
            if (product == null)
            {
                context.Result = new NotFoundObjectResult($"Product with ID {id} not found.");
            }
        }

        private async Task ValidateCategory(ActionExecutingContext context, int id)
        {
            var categoryRepository = context.HttpContext.RequestServices.GetService<ICategoryRepository>();
            var category = await categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                context.Result = new NotFoundObjectResult($"Category with ID {id} not found.");
                return;
            }
        }

        private void ValidateUser(ActionExecutingContext context, int id)
        {
            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = userRepository?.GetUserById(id);
            if (user == null)
            {
                context.Result = new NotFoundObjectResult($"User with ID {id} not found.");
                return;
            }
        }

        private void ValidateOrder(ActionExecutingContext context, int id)
        {
            var orderRepository = context.HttpContext.RequestServices.GetService<IOrderRepository>();
            var order = orderRepository?.GetOrderById(id);
            if (order == null)
            {
                context.Result = new NotFoundObjectResult($"Order with ID {id} not found.");
                return;
            }
        }

        private async Task ValidateUserReview(ActionExecutingContext context, int id)
        {
            var reviewRepository = context.HttpContext.RequestServices.GetService<IUserReviewRepository>();
            var review = await reviewRepository.GetReviewById(id);
            if (review == null)
            {
                context.Result = new NotFoundObjectResult($"User Review with ID {id} not found.");
            }
        }
    }
}
