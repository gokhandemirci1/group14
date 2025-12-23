using BudgetTracker.Models;

namespace BudgetTracker.Services;

/// <summary>
/// Kategori işlemleri için Service Interface.
/// </summary>
public interface ICategoryService
{
    Task<IEnumerable<Category>> GetUserCategoriesAsync(int userId);
    Task<Category?> GetCategoryByIdAsync(int categoryId, int userId);
    Task<Category> CreateCategoryAsync(string name, int userId, string? description = null, string? color = null);
    Task<bool> UpdateCategoryAsync(int categoryId, int userId, string name, string? description = null, string? color = null);
    Task<bool> DeleteCategoryAsync(int categoryId, int userId);
    Task<bool> CategoryBelongsToUserAsync(int categoryId, int userId);
}





