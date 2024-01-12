namespace XavierPlayLandAPI.Models.Repositories
{
    public interface IUserReviewRepository
    {
        Task<IEnumerable<UserReview>> GetAllReviews();
        Task<UserReview?> GetReviewById(int id);
        Task AddReview (UserReview userReview);
        Task UpdateReview (UserReview userReview);
        Task DeleteReview (int id);
        bool ReviewExists(int id);
        
    }
}
