namespace BudgetTracker.Models;

/// <summary>
/// Kullanıcı entity sınıfı.
/// Encapsulation: Tüm property'ler private set ile korunuyor.
/// </summary>
public class User
{
    // Primary Key
    public int Id { get; private set; }

    // Kullanıcı adı (unique olmalı)
    public string Username { get; private set; } = string.Empty;

    // Şifre (hash'lenmiş olarak saklanacak)
    public string PasswordHash { get; private set; } = string.Empty;

    // Oluşturulma tarihi
    public DateTime CreatedAt { get; private set; }

    // Navigation Property: Bir kullanıcının birden fazla harcaması olabilir
    public virtual ICollection<Expense> Expenses { get; private set; } = new List<Expense>();

    // Navigation Property: Bir kullanıcının birden fazla kategorisi olabilir
    public virtual ICollection<Category> Categories { get; private set; } = new List<Category>();

    // Private constructor - EF Core için
    private User() { }

    // Public constructor - Yeni kullanıcı oluşturmak için
    public User(string username, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("PasswordHash cannot be null or empty.", nameof(passwordHash));

        Username = username;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    // Method: Şifre güncelleme (Encapsulation)
    public void UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("PasswordHash cannot be null or empty.", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;
    }
}





