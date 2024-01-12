namespace XavierPlayLandAPI.Models.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Description = "desc 1", Brand = "brand 1", CategoryId = 1, Price = 10.99, Quantity = 5, isAvailable = true, ReleaseDate = DateOnly.FromDateTime(new DateTime(2022, 1, 1)), ImagePath = "image1.jpg" },
            new Product { Id = 2, Name = "Product 2", Description = "desc 2", Brand = "brand 2", CategoryId = 2, Price = 15.50, Quantity = 3, isAvailable = true, ReleaseDate = DateOnly.FromDateTime(new DateTime(2022, 2, 1)), ImagePath = "image2.jpg" },
            new Product { Id = 3, Name = "Product 3", Description = "desc 3", Brand = "brand 3", CategoryId = 3, Price = 23.99, Quantity = 1, isAvailable = true, ReleaseDate = DateOnly.FromDateTime(new DateTime(2022, 3, 1)), ImagePath = "image3.jpg" }
        };

        public bool ProductExists(int productId)
        {
            // check if the product exists
            return _products.Any(p => p.Id == productId);
        }

        public Task<bool> AnyProductWithCategoryId(int categoryId)
        {
            // checks for product with an existing categoryid
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

            if (product.Quantity > 0)
            {
                product.isAvailable = true;
            }
            else
            {
                product.isAvailable = false;
            }

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
                existingProduct.isAvailable = existingProduct.isAvailable;
                existingProduct.ReleaseDate = existingProduct.ReleaseDate;
                existingProduct.ImagePath = product.ImagePath;
            }

            // set the isavailable bool to true if greater than 0,
            // otherwise set it back to false
            if (existingProduct?.Quantity > 0)
            {
                product.isAvailable = true;
                existingProduct.isAvailable = product.isAvailable;
            }
            else
            {
                product.isAvailable = false;
                existingProduct.isAvailable = product.isAvailable;
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
