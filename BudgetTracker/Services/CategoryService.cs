using BudgetTracker.Data;
using BudgetTracker.Models;

namespace BudgetTracker.Services;

/// <summary>
/// Kategori işlemleri için Service Implementation.
/// Business logic burada - Controller direkt repository'ye erişmez.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;

    public CategoryService(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<IEnumerable<Category>> GetUserCategoriesAsync(int userId)
    {
        return await _categoryRepository.FindAsync(c => c.UserId == userId);
    }

    public async Task<Category?> GetCategoryByIdAsync(int categoryId, int userId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        
        // Kategori kullanıcıya ait mi kontrol et
        if (category == null || category.UserId != userId)
            return null;

        return category;
    }

    public async Task<Category> CreateCategoryAsync(string name, int userId, string? description = null, string? color = null)
    {
        var category = new Category(name, userId, description, color ?? "#007bff");
        return await _categoryRepository.AddAsync(category);
    }

    public async Task<bool> UpdateCategoryAsync(int categoryId, int userId, string name, string? description = null, string? color = null)
    {
        var category = await GetCategoryByIdAsync(categoryId, userId);
        if (category == null)
            return false;

        category.Update(name, description, color);
        _categoryRepository.Update(category);
        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId, int userId)
    {
        var category = await GetCategoryByIdAsync(categoryId, userId);
        if (category == null)
            return false;

        _categoryRepository.Delete(category);
        return true;
    }

    public async Task<bool> CategoryBelongsToUserAsync(int categoryId, int userId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        return category != null && category.UserId == userId;
    }
}





