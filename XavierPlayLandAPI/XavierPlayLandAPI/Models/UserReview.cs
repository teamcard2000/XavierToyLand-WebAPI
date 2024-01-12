using System.ComponentModel.DataAnnotations;
using XavierPlayLandAPI.Filters;

namespace XavierPlayLandAPI.Models
{
    public class UserReview : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        [Rating(1)]
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public DateOnly DateCreated { get; set; }
    }
}
