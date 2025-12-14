using BudgetTracker.Data;
using BudgetTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Services;

/// <summary>
/// Harcama işlemleri için Service Implementation.
/// </summary>
public class ExpenseService : IExpenseService
{
    private readonly IRepository<Expense> _expenseRepository;
    private readonly ICategoryService _categoryService;
    private readonly ApplicationDbContext _context;

    public ExpenseService(
        IRepository<Expense> expenseRepository,
        ICategoryService categoryService,
        ApplicationDbContext context)
    {
        _expenseRepository = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Expense>> GetUserExpensesAsync(int userId)
    {
        return await _context.Expenses
            .Include(e => e.Category)
            .Where(e => e.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetUserExpensesByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
    {
        return await _context.Expenses
            .Include(e => e.Category)
            .Where(e => e.UserId == userId && 
                       e.ExpenseDate >= startDate && 
                       e.ExpenseDate <= endDate)
            .ToListAsync();
    }

    public async Task<Expense?> GetExpenseByIdAsync(int expenseId, int userId)
    {
        var expense = await _context.Expenses
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == expenseId);
        
        // Harcama kullanıcıya ait mi kontrol et
        if (expense == null || expense.UserId != userId)
            return null;

        return expense;
    }

    public async Task<Expense> CreateExpenseAsync(decimal amount, string description, DateTime expenseDate, int userId, int categoryId)
    {
        // Kategori kullanıcıya ait mi kontrol et
        if (!await _categoryService.CategoryBelongsToUserAsync(categoryId, userId))
            throw new UnauthorizedAccessException("Category does not belong to user");

        var expense = new Expense(amount, description, expenseDate, userId, categoryId);
        return await _expenseRepository.AddAsync(expense);
    }

    public async Task<bool> UpdateExpenseAsync(int expenseId, int userId, decimal amount, string description, DateTime expenseDate, int categoryId)
    {
        var expense = await GetExpenseByIdAsync(expenseId, userId);
        if (expense == null)
            return false;

        // Kategori kullanıcıya ait mi kontrol et
        if (!await _categoryService.CategoryBelongsToUserAsync(categoryId, userId))
            return false;

        expense.Update(amount, description, expenseDate, categoryId);
        _expenseRepository.Update(expense);
        return true;
    }

    public async Task<bool> DeleteExpenseAsync(int expenseId, int userId)
    {
        var expense = await GetExpenseByIdAsync(expenseId, userId);
        if (expense == null)
            return false;

        _expenseRepository.Delete(expense);
        return true;
    }

    public async Task<decimal> GetTotalExpensesByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
    {
        var expenses = await GetUserExpensesByDateRangeAsync(userId, startDate, endDate);
        return expenses.Sum(e => e.Amount);
    }

    public async Task<Dictionary<string, decimal>> GetExpensesByCategoryAsync(int userId, DateTime startDate, DateTime endDate)
    {
        var expenses = await _context.Expenses
            .Include(e => e.Category)
            .Where(e => e.UserId == userId && 
                       e.ExpenseDate >= startDate && 
                       e.ExpenseDate <= endDate)
            .ToListAsync();

        return expenses
            .GroupBy(e => e.Category.Name)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
    }
}

