using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XavierPlayLandAPI.Models.Repositories;
using XavierPlayLandAPI.Models;
using System.Security.Principal;

namespace XavierPlayLandAPI.Filters.ActionFilters
{
    public class ValidateUpdateEntityFilterAttribute : ActionFilterAttribute
    {
        private readonly EntityType _entityType;

        public ValidateUpdateEntityFilterAttribute(EntityType entityType)
        {
            _entityType = entityType;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            string entityArgumentKey = _entityType.ToString().ToLower(); // This should match the parameter name of the action method.

            if (!context.ActionArguments.ContainsKey(entityArgumentKey))
            {
                context.Result = new BadRequestObjectResult($"{_entityType} data is missing or invalid.");
                return;
            }

            // Assuming all your entities have an 'Id' property and they are of type IEntity
            if (context.ActionArguments[entityArgumentKey] is not IEntity entity || entity.Id <= 0)
            {
                context.Result = new BadRequestObjectResult($"Invalid {_entityType} ID.");
                return;
            }

            switch (_entityType)
            {
                case EntityType.Product:
                    await ValidateProduct(context, entity.Id);
                    break;
                case EntityType.Category:
                    await ValidateCategory(context, entity.Id);
                    break;
                case EntityType.User:
                    ValidateUser(context, entity.Id);
                    break;
                case EntityType.Order:
                    ValidateOrder(context, entity.Id);
                    break;
                case EntityType.Review:
                    await ValidateUserReview(context, entity.Id);
                    break;
                // Add other cases as necessary
                default:
                    context.Result = new BadRequestObjectResult("Invalid entity type.");
                    return;
            }

            base.OnActionExecuting(context);
        }


        private async Task ValidateProduct(ActionExecutingContext context, int id)
        {
            var productRepository = context.HttpContext.RequestServices.GetService<IProductRepository>();
            var existingProduct = await productRepository.GetProductById(id);
            if (existingProduct == null)
            {
                context.Result = new NotFoundObjectResult($"Product item with ID {id} not found.");
                return;
            }
        }
        private async Task ValidateCategory(ActionExecutingContext context, int id)
        {
            var categoryRepository = context.HttpContext.RequestServices.GetService<ICategoryRepository>();
            var existingCategory = await categoryRepository.GetCategoryById(id);
            if (existingCategory == null)
            {
                context.Result = new NotFoundObjectResult($"Category item with ID {id} not found.");
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
            var existingReview = await reviewRepository.GetReviewById(id);
            if (existingReview == null)
            {
                context.Result = new NotFoundObjectResult($"User Review with ID {id} not found.");
                return;
            }
        }
    }
}
