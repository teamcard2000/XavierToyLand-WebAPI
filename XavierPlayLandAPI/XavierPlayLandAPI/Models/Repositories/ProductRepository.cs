namespace XavierPlayLandAPI.Models.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static List<Product> _products = new List<Product>
        {
            // Example products - you can add more or start with an empty list
            new Product { Id = 1, Name = "Product 1", Description = "desc 1", Brand = "brand 1", CategoryId = 1, Price = 10.99, Quantity = 5, ImagePath = "image1.jpg" },
            new Product { Id = 2, Name = "Product 2", Description = "desc 2", Brand = "brand 2", CategoryId = 2, Price = 15.50, Quantity = 3, ImagePath = "image2.jpg" },
            new Product { Id = 3, Name = "Product 3", Description = "desc 3", Brand = "brand 3", CategoryId = 3, Price = 23.99, Quantity = 1, ImagePath = "image3.jpg" }
        };

        public bool ProductExists(int productId)
        {
            return _products.Any(p => p.Id == productId);
        }

        public Task<bool> AnyProductWithCategoryId(int categoryId)
        {
            bool exists = _products.Any(p => p.CategoryId == categoryId);
            return Task.FromResult(exists);
        }

        public Task<IEnumerable<Product>> GetAllProducts()
        {
            return Task.FromResult(_products.AsEnumerable());
        }

        public Task<Product?> GetProductById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public Task AddProduct(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1; // Auto-increment ID
            _products.Add(product);
            return Task.CompletedTask;
        }

        public Task UpdateProduct(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Id = product.Id;
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Brand = product.Brand;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
                existingProduct.ImagePath = product.ImagePath;
            }
            return Task.CompletedTask;
        }

        public Task DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
            return Task.CompletedTask;
        }
    }
}
