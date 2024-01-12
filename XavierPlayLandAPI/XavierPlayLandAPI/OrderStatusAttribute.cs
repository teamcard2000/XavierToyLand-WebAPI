using System.ComponentModel.DataAnnotations;

namespace XavierPlayLandAPI
{
    public class OrderStatusAttribute : ValidationAttribute
    {
        private readonly string[] _orderStatus = ["Ordered", "Shipped", "Out For Delivery", "Delivered", "Cancelled"];

        public OrderStatusAttribute() 
        {
            ErrorMessage = "You must use those following terms when creating a new order: 'Ordered', 'Shipped', 'Out For Delivery', 'Delivered', 'Cancelled'";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Check if the value is a string and one of the order status terms
            if (value is string orderStatus && _orderStatus.Contains(orderStatus))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
