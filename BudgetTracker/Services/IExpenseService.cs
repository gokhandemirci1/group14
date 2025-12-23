using BudgetTracker.Models;

namespace BudgetTracker.Services;

/// <summary>
/// Harcama işlemleri için Service Interface.
/// </summary>
public interface IExpenseService
{
    Task<IEnumerable<Expense>> GetUserExpensesAsync(int userId);
    Task<IEnumerable<Expense>> GetUserExpensesByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
    Task<Expense?> GetExpenseByIdAsync(int expenseId, int userId);
    Task<Expense> CreateExpenseAsync(decimal amount, string description, DateTime expenseDate, int userId, int categoryId);
    Task<bool> UpdateExpenseAsync(int expenseId, int userId, decimal amount, string description, DateTime expenseDate, int categoryId);
    Task<bool> DeleteExpenseAsync(int expenseId, int userId);
    Task<decimal> GetTotalExpensesByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
    Task<Dictionary<string, decimal>> GetExpensesByCategoryAsync(int userId, DateTime startDate, DateTime endDate);
}





