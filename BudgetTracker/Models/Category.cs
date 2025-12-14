namespace BudgetTracker.Models;

/// <summary>
/// Harcama kategorisi entity sınıfı.
/// Her kullanıcı kendi kategorilerini oluşturabilir.
/// </summary>
public class Category
{
    // Primary Key
    public int Id { get; private set; }

    // Kategori adı
    public string Name { get; private set; } = string.Empty;

    // Kategori açıklaması (opsiyonel)
    public string? Description { get; private set; }

    // Kategori rengi (UI'da gösterim için - hex format: #FF5733)
    public string Color { get; private set; } = "#007bff";

    // Oluşturulma tarihi
    public DateTime CreatedAt { get; private set; }

    // Foreign Key: Hangi kullanıcıya ait
    public int UserId { get; private set; }

    // Navigation Property: Kategoriye ait kullanıcı
    public virtual User User { get; private set; } = null!;

    // Navigation Property: Bu kategoriye ait harcamalar
    public virtual ICollection<Expense> Expenses { get; private set; } = new List<Expense>();

    // Private constructor - EF Core için
    private Category() { }

    // Public constructor - Yeni kategori oluşturmak için
    public Category(string name, int userId, string? description = null, string color = "#007bff")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be null or empty.", nameof(name));

        if (userId <= 0)
            throw new ArgumentException("UserId must be greater than zero.", nameof(userId));

        Name = name;
        UserId = userId;
        Description = description;
        Color = color;
        CreatedAt = DateTime.UtcNow;
    }

    // Method: Kategori bilgilerini güncelleme (Encapsulation)
    public void Update(string name, string? description = null, string? color = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be null or empty.", nameof(name));

        Name = name;
        Description = description;
        
        if (!string.IsNullOrWhiteSpace(color))
        {
            Color = color;
        }
    }
}




