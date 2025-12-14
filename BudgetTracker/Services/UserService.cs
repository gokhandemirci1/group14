using BudgetTracker.Data;
using BudgetTracker.Models;
using System.Security.Cryptography;
using System.Text;

namespace BudgetTracker.Services;

/// <summary>
/// Kullanıcı işlemleri için Service Implementation.
/// SOLID - Single Responsibility: Sadece kullanıcı iş mantığından sorumlu.
/// </summary>
public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<User?> RegisterAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return null;

        // Kullanıcı adı zaten var mı kontrol et
        if (await UsernameExistsAsync(username))
            return null;

        // Şifreyi hash'le
        string passwordHash = HashPassword(password);

        // Yeni kullanıcı oluştur
        var user = new User(username, passwordHash);
        return await _userRepository.AddAsync(user);
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return null;

        // Kullanıcıyı bul
        var user = await _userRepository.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
            return null;

        // Şifreyi doğrula
        if (VerifyPassword(password, user.PasswordHash))
            return user;

        return null;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _userRepository.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _userRepository.ExistsAsync(u => u.Username == username);
    }

    // Private Helper Methods
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == passwordHash;
    }
}




