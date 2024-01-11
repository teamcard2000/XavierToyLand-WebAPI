using System.ComponentModel.DataAnnotations;

namespace XavierPlayLandAPI
{
    public class MinimumPriceAttribute : ValidationAttribute
    {
        private readonly double _minimumPrice;

        public MinimumPriceAttribute(double minimumPrice)
        {
            _minimumPrice = minimumPrice;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is double price && price < _minimumPrice)
            {
                return new ValidationResult($"Price must be at least {_minimumPrice} or above.");
            }

            return ValidationResult.Success;
        }
    }
}
