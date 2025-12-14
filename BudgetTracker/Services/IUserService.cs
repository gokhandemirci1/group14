using BudgetTracker.Models;

namespace BudgetTracker.Services;

/// <summary>
/// Kullanıcı işlemleri için Service Interface.
/// SOLID - Interface Segregation Principle
/// </summary>
public interface IUserService
{
    Task<User?> RegisterAsync(string username, string password);
    Task<User?> LoginAsync(string username, string password);
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<bool> UsernameExistsAsync(string username);
}




