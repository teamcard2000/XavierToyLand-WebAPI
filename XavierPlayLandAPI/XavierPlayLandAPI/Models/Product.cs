using XavierPlayLandAPI.Filters;

namespace XavierPlayLandAPI.Models
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public int? CategoryId { get; set; }
        [MinimumPrice(5.99)]
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public bool? isAvailable { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public string? ImagePath { get; set; }
    }
}
