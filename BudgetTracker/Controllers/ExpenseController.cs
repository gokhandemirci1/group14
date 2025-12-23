using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BudgetTracker.Services;

namespace BudgetTracker.Controllers;

/// <summary>
/// Harcama işlemleri için Controller.
/// </summary>
[Authorize]
public class ExpenseController : Controller
{
    private readonly IExpenseService _expenseService;
    private readonly ICategoryService _categoryService;

    public ExpenseController(IExpenseService expenseService, ICategoryService categoryService)
    {
        _expenseService = expenseService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var expenses = await _expenseService.GetUserExpensesAsync(userId.Value);
        return View(expenses.OrderByDescending(e => e.ExpenseDate));
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var categories = await _categoryService.GetUserCategoriesAsync(userId.Value);
        ViewBag.Categories = categories;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(decimal amount, string description, DateTime expenseDate, int categoryId)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        if (amount <= 0)
        {
            ViewBag.Error = "Tutar 0'dan büyük olmalıdır.";
            var categories = await _categoryService.GetUserCategoriesAsync(userId.Value);
            ViewBag.Categories = categories;
            return View();
        }

        try
        {
            await _expenseService.CreateExpenseAsync(amount, description, expenseDate, userId.Value, categoryId);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            var categories = await _categoryService.GetUserCategoriesAsync(userId.Value);
            ViewBag.Categories = categories;
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var expense = await _expenseService.GetExpenseByIdAsync(id, userId.Value);
        if (expense == null)
            return NotFound();

        var categories = await _categoryService.GetUserCategoriesAsync(userId.Value);
        ViewBag.Categories = categories;
        return View(expense);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, decimal amount, string description, DateTime expenseDate, int categoryId)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        if (amount <= 0)
        {
            ViewBag.Error = "Tutar 0'dan büyük olmalıdır.";
            var expense = await _expenseService.GetExpenseByIdAsync(id, userId.Value);
            var categories = await _categoryService.GetUserCategoriesAsync(userId.Value);
            ViewBag.Categories = categories;
            return View(expense);
        }

        var success = await _expenseService.UpdateExpenseAsync(id, userId.Value, amount, description, expenseDate, categoryId);
        if (!success)
            return NotFound();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var success = await _expenseService.DeleteExpenseAsync(id, userId.Value);
        if (!success)
            return NotFound();

        return RedirectToAction("Index");
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userIdClaim != null ? int.Parse(userIdClaim) : null;
    }
}





