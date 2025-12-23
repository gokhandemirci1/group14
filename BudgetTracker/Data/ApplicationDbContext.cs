using Microsoft.EntityFrameworkCore;
using BudgetTracker.Models;

namespace BudgetTracker.Data;

/// <summary>
/// Entity Framework Core DbContext sınıfı.
/// Veritabanı bağlantısı ve entity konfigürasyonları burada yapılır.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSet'ler - Veritabanı tablolarını temsil eder
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Entity Konfigürasyonu
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            // Username unique olmalı
            entity.HasIndex(e => e.Username)
                .IsUnique();

            // Bir kullanıcının birden fazla harcaması olabilir
            entity.HasMany(e => e.Expenses)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bir kullanıcının birden fazla kategorisi olabilir
            entity.HasMany(e => e.Categories)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Category Entity Konfigürasyonu
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Description)
                .HasMaxLength(500);
            
            entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(7)
                .HasDefaultValue("#007bff");

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            // Bir kategorinin birden fazla harcaması olabilir
            entity.HasMany(e => e.Expenses)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Kategori silinirse harcamalar silinmesin
        });

        // Expense Entity Konfigürasyonu
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount)
                .IsRequired()
                .HasPrecision(18, 2); // Para tutarı için precision
            
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(e => e.ExpenseDate)
                .IsRequired();
            
            entity.Property(e => e.CreatedAt)
                .IsRequired();

            // ExpenseDate için index (sorgu performansı için)
            entity.HasIndex(e => e.ExpenseDate);
            entity.HasIndex(e => new { e.UserId, e.ExpenseDate });
        });
    }
}





