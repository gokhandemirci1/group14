using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BudgetTracker.Services;

namespace BudgetTracker.Controllers;

/// <summary>
/// Kategori işlemleri için Controller.
/// </summary>
[Authorize]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var categories = await _categoryService.GetUserCategoriesAsync(userId.Value);
        return View(categories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string name, string? description, string? color)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        if (string.IsNullOrWhiteSpace(name))
        {
            ViewBag.Error = "Kategori adı gereklidir.";
            return View();
        }

        await _categoryService.CreateCategoryAsync(name, userId.Value, description, color);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var category = await _categoryService.GetCategoryByIdAsync(id, userId.Value);
        if (category == null)
            return NotFound();

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, string name, string? description, string? color)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        if (string.IsNullOrWhiteSpace(name))
        {
            ViewBag.Error = "Kategori adı gereklidir.";
            var category = await _categoryService.GetCategoryByIdAsync(id, userId.Value);
            return View(category);
        }

        var success = await _categoryService.UpdateCategoryAsync(id, userId.Value, name, description, color);
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

        var success = await _categoryService.DeleteCategoryAsync(id, userId.Value);
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




