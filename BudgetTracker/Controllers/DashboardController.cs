using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BudgetTracker.Services;

namespace BudgetTracker.Controllers;

/// <summary>
/// Dashboard Controller - Ana sayfa ve raporlama.
/// [Authorize] attribute ile sadece giriş yapmış kullanıcılar erişebilir.
/// </summary>
[Authorize]
public class DashboardController : Controller
{
    private readonly IExpenseService _expenseService;
    private readonly ICategoryService _categoryService;

    public DashboardController(IExpenseService expenseService, ICategoryService categoryService)
    {
        _expenseService = expenseService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        // Tarih aralıkları
        var now = DateTime.Now;
        var startOfWeek = now.AddDays(-(int)now.DayOfWeek);
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var startOfSixMonths = now.AddMonths(-6);

        // Haftalık, aylık ve 6 aylık toplamlar
        var weeklyTotal = await _expenseService.GetTotalExpensesByDateRangeAsync(userId.Value, startOfWeek, now);
        var monthlyTotal = await _expenseService.GetTotalExpensesByDateRangeAsync(userId.Value, startOfMonth, now);
        var sixMonthsTotal = await _expenseService.GetTotalExpensesByDateRangeAsync(userId.Value, startOfSixMonths, now);

        // Kategori bazında harcamalar
        var weeklyByCategory = await _expenseService.GetExpensesByCategoryAsync(userId.Value, startOfWeek, now);
        var monthlyByCategory = await _expenseService.GetExpensesByCategoryAsync(userId.Value, startOfMonth, now);
        var sixMonthsByCategory = await _expenseService.GetExpensesByCategoryAsync(userId.Value, startOfSixMonths, now);

        ViewBag.WeeklyTotal = weeklyTotal;
        ViewBag.MonthlyTotal = monthlyTotal;
        ViewBag.SixMonthsTotal = sixMonthsTotal;
        ViewBag.WeeklyByCategory = weeklyByCategory;
        ViewBag.MonthlyByCategory = monthlyByCategory;
        ViewBag.SixMonthsByCategory = sixMonthsByCategory;

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetChartData(string period)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Json(new { error = "Unauthorized" });

        var now = DateTime.Now;
        DateTime startDate;
        Dictionary<string, decimal> data;

        switch (period.ToLower())
        {
            case "weekly":
                startDate = now.AddDays(-(int)now.DayOfWeek);
                data = await _expenseService.GetExpensesByCategoryAsync(userId.Value, startDate, now);
                break;
            case "monthly":
                startDate = new DateTime(now.Year, now.Month, 1);
                data = await _expenseService.GetExpensesByCategoryAsync(userId.Value, startDate, now);
                break;
            case "sixmonths":
                startDate = now.AddMonths(-6);
                data = await _expenseService.GetExpensesByCategoryAsync(userId.Value, startDate, now);
                break;
            default:
                return Json(new { error = "Invalid period" });
        }

        return Json(new
        {
            labels = data.Keys.ToArray(),
            values = data.Values.ToArray()
        });
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userIdClaim != null ? int.Parse(userIdClaim) : null;
    }
}





