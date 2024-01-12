﻿namespace XavierPlayLandAPI.Models.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int id);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
        Task<bool> AnyProductWithCategoryId(int id);
        bool ProductExists(int productId);
    }
}
