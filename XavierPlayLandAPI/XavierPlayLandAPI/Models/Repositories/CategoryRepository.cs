namespace XavierPlayLandAPI.Models.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private static List<Category> _categories = new List<Category> 
        { 
            new Category { Id = 1, Name = "Category 1" },
            new Category { Id = 2, Name = "Category 2" },
            new Category { Id = 3, Name = "Category 3" },
            new Category { Id = 4, Name = "Category 4" },
            new Category { Id = 5, Name = "Category 5" }
        };

        public Task<bool> CategoryExists(int id)
        {
            bool exists = _categories.Any(c => c.Id == id);
            return Task.FromResult(exists);
        }

        public Task<IEnumerable<Category>> GetAllCategories()
        {
            return Task.FromResult(_categories.AsEnumerable());
        }

        public Task<Category?> GetCategoryById(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(category);
        }

        public Task AddCategory(Category category)
        {
            category.Id = _categories.Max(c => c.Id) + 1; // Auto-increment ID
            _categories.Add(category);
            return Task.CompletedTask;
        }

        public Task UpdateCategory(Category category)
        {
            var existingCategory = _categories.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory != null)
            {
                existingCategory.Id = category.Id;
                existingCategory.Name = category.Name;
            }
            return Task.CompletedTask;
        }

        public Task DeleteCategory(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _categories.Remove(category);
            }
            return Task.CompletedTask;
        }
    }
}
