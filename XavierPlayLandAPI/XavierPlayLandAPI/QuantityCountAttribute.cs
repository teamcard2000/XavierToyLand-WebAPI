using System.ComponentModel.DataAnnotations;

namespace XavierPlayLandAPI
{
    public class QuantityCountAttribute : ValidationAttribute
    {
        private readonly int _quantityCount;

        public QuantityCountAttribute(int quantityCount)
        {
            _quantityCount = quantityCount;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int quantity && quantity < _quantityCount)
            {
                return new ValidationResult($"Quantity count must be atleast {_quantityCount} or above.");
            }

            return ValidationResult.Success;
        }
    }
}
