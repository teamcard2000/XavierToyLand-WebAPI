using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XavierPlayLandAPI.Filters;
using XavierPlayLandAPI.Filters.ActionFilters;
using XavierPlayLandAPI.Filters.ExceptionFilters;
using XavierPlayLandAPI.Models;
using XavierPlayLandAPI.Models.Repositories;

namespace XavierPlayLandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserReviewsController : ControllerBase
    {
        private readonly IUserReviewRepository _userReviewRepository;

        public UserReviewsController(IUserReviewRepository userReviewRepository)
        {
            _userReviewRepository = userReviewRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            return Ok(await _userReviewRepository.GetAllReviews());
        }

        [HttpGet("{id}")]
        [ValidateEntityIdFilter(EntityType.Review)]
        public async Task<IActionResult> GetReview(int id)
        {
            return Ok(await _userReviewRepository.GetReviewById(id));
        }

        [HttpPost]
        [ValidateAddEntityFilter(EntityType.Review)]
        public async Task<IActionResult> CreateReview(UserReview review)
        {
            await _userReviewRepository.AddReview(review);
            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        [ValidateEntityIdFilter(EntityType.Review)]
        [ValidateUpdateEntityFilter(EntityType.Review)]
        [HandleUpdateExceptionsFilter]
        public async Task<IActionResult> UpdateReview(int id, UserReview review)
        {
            if (id != review.Id)
            {
                return BadRequest("UserReview ID mismatch!");
            }

            await _userReviewRepository.UpdateReview(review);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ValidateEntityIdFilter(EntityType.Review)]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await _userReviewRepository.DeleteReview(id);
            return Ok();
        }
    }
}
