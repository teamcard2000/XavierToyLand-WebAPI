using System.ComponentModel.DataAnnotations;

namespace XavierPlayLandAPI
{
    public class RatingAttribute : ValidationAttribute
    {
        private readonly int _rating;

        public RatingAttribute(int rating)
        {
            _rating = rating;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int rating && rating < _rating)
            {
                return new ValidationResult($"Rating must be at least {_rating} star or above");
            }

            return ValidationResult.Success;
        }
    }
}
