using System.Security.Cryptography.X509Certificates;

namespace XavierPlayLandAPI.Models.Repositories
{
    public class UserReviewRepository : IUserReviewRepository
    {
        private readonly IProductRepository _productRepository;

        public UserReviewRepository(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        private static List<UserReview> _userReviews = new List<UserReview>
        {
            new UserReview { Id = 1, UserId = 1, ProductId = 1, Rating = 5, Title = "PRODUCT 1 REVIEW", Comment = "Best Product Ever!", DateCreated = DateOnly.FromDateTime(DateTime.Now) },
            new UserReview { Id = 2, UserId = 2, ProductId = 1, Rating = 1, Title = "Worst Product Ever", Comment = "Ppl only buy this cuz theyre geeks!", DateCreated = DateOnly.FromDateTime(DateTime.Now) },
            new UserReview { Id = 3, UserId = 2, ProductId = 2, Rating = 3, Title = "Ehh couldve been better", Comment = "It was okay!", DateCreated = DateOnly.FromDateTime(DateTime.Now) },
            new UserReview { Id = 4, UserId = 1, ProductId = 3, Rating = 5, Title = "PRODUCT 3 REVIEW", Comment = "Im only an admin after all just writing those reviews for testing purposes hahe", DateCreated = DateOnly.FromDateTime(DateTime.Now) },
            new UserReview { Id = 5, UserId = 1, ProductId = 4, Rating = 5, Title = "PRODUCT 4 REVIEW", Comment = "Same goes with this lol!", DateCreated = DateOnly.FromDateTime(DateTime.Now) },
        };

        public bool ReviewExists(int id)
        {
            return _userReviews.Any(r => r.Id == id);
        }

        public Task<IEnumerable<UserReview>> GetAllReviews()
        {
            return Task.FromResult(_userReviews.AsEnumerable());
        }

        public Task<UserReview?> GetReviewById(int id)
        {
            var review = _userReviews.FirstOrDefault(r  => r.Id == id);
            return Task.FromResult(review);
        }

        public Task AddReview(UserReview review)
        {
            review.Id = _userReviews.Max(r => r.Id) + 1;
            _userReviews.Add(review);

            var userExists = TemporaryUsers.Users.Any(u => u.Id == review.UserId);
            if (!userExists)
            {
                throw new ArgumentException("User does not exist.");
            }

            var productExists = _productRepository.ProductExists(review.ProductId);
            if (!productExists)
            {
                throw new ArgumentException($"Product ID {review.ProductId} does not exist");
            }

            return Task.CompletedTask;
        }

        public Task UpdateReview(UserReview review)
        {
            var userExists = TemporaryUsers.Users.Any(u => u.Id == review.UserId);
            if (!userExists)
            {
                throw new ArgumentException("User does not exist.");
            }

            var productExists = _productRepository.ProductExists(review.ProductId);
            if (!productExists)
            {
                throw new ArgumentException($"Product ID {review.ProductId} does not exist");
            }

            var existingReview = _userReviews.FirstOrDefault(r =>r.Id == review.Id);
            if (existingReview != null)
            {
                existingReview.UserId = review.UserId;
                existingReview.ProductId = review.ProductId;
                existingReview.Rating = review.Rating;
                existingReview.Title = review.Title;
                existingReview.Comment = review.Comment;
            }
            return Task.CompletedTask;
        }

        public Task DeleteReview(int id)
        {
            var review = _userReviews.FirstOrDefault(r =>r.Id == id);
            if (review != null)
            {
                _userReviews.Remove(review);
            }
            return Task.CompletedTask;
        }
    }
}
