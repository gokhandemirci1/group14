namespace BudgetTracker.Models;

/// <summary>
/// Harcama entity sınıfı.
/// Kullanıcılar geçmiş ve gelecek tarihli harcamalar ekleyebilir.
/// </summary>
public class Expense
{
    // Primary Key
    public int Id { get; private set; }

    // Harcama tutarı (pozitif değer olmalı)
    public decimal Amount { get; private set; }

    // Harcama açıklaması
    public string Description { get; private set; } = string.Empty;

    // Harcama tarihi (geçmiş veya gelecek olabilir)
    public DateTime ExpenseDate { get; private set; }

    // Oluşturulma tarihi (kayıt ne zaman eklendi)
    public DateTime CreatedAt { get; private set; }

    // Foreign Key: Hangi kullanıcıya ait
    public int UserId { get; private set; }

    // Foreign Key: Hangi kategoriye ait
    public int CategoryId { get; private set; }

    // Navigation Property: Harcamaya ait kullanıcı
    public virtual User User { get; private set; } = null!;

    // Navigation Property: Harcamaya ait kategori
    public virtual Category Category { get; private set; } = null!;

    // Private constructor - EF Core için
    private Expense() { }

    // Public constructor - Yeni harcama oluşturmak için
    public Expense(decimal amount, string description, DateTime expenseDate, int userId, int categoryId)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or empty.", nameof(description));

        if (userId <= 0)
            throw new ArgumentException("UserId must be greater than zero.", nameof(userId));

        if (categoryId <= 0)
            throw new ArgumentException("CategoryId must be greater than zero.", nameof(categoryId));

        Amount = amount;
        Description = description;
        ExpenseDate = expenseDate;
        UserId = userId;
        CategoryId = categoryId;
        CreatedAt = DateTime.UtcNow;
    }

    // Method: Harcama bilgilerini güncelleme (Encapsulation)
    public void Update(decimal amount, string description, DateTime expenseDate, int categoryId)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or empty.", nameof(description));

        if (categoryId <= 0)
            throw new ArgumentException("CategoryId must be greater than zero.", nameof(categoryId));

        Amount = amount;
        Description = description;
        ExpenseDate = expenseDate;
        CategoryId = categoryId;
    }
}




