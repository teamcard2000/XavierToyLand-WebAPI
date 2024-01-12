using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace XavierPlayLandAPI
{
    public class PaymentMethodAttribute : ValidationAttribute
    {
        private readonly string[] _validMethods = ["Visa", "Mastercard", "Amex", "Paypal"];

        public PaymentMethodAttribute()
        {
            ErrorMessage = "Invalid payment method. Only Visa, Mastercard, Amex, and Paypal are accepted.";
        }

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            // Check if the value is a string and one of the valid methods
            if (value is string paymentMethod && _validMethods.Contains(paymentMethod))
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
