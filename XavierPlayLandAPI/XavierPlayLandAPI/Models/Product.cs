namespace XavierPlayLandAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        // relationship with category
        public int? CategoryId { get; set; }
        [MinimumPrice(5.99)]
        public double? Price { get; set; }
        public string? ImagePath { get; set; }
    }
}
