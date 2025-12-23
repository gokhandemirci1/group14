using System.Linq.Expressions;

namespace BudgetTracker.Data;

/// <summary>
/// Generic Repository Pattern Interface.
/// SOLID - Interface Segregation ve Dependency Inversion prensiplerine uygun.
/// Tüm repository'ler için ortak CRUD operasyonlarını tanımlar.
/// </summary>
/// <typeparam name="T">Entity tipi</typeparam>
public interface IRepository<T> where T : class
{
    // Tüm kayıtları getir
    Task<IEnumerable<T>> GetAllAsync();

    // ID'ye göre kayıt getir
    Task<T?> GetByIdAsync(int id);

    // Koşula göre kayıtları getir
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    // Koşula göre tek kayıt getir
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    // Yeni kayıt ekle
    Task<T> AddAsync(T entity);

    // Kayıt güncelle
    void Update(T entity);

    // Kayıt sil
    void Delete(T entity);

    // ID'ye göre kayıt sil
    Task<bool> DeleteByIdAsync(int id);

    // Kayıt sayısını getir
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

    // Kayıt var mı kontrol et
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}





