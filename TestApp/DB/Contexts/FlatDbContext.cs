using Microsoft.EntityFrameworkCore;
using TestApp.DB.Model;

namespace TestApp.DB.Contexts;

/// <summary>
/// Контекст базы данных, предоставляющий доступ ко всем таблицам.
/// </summary>
public class FlatDbContext(DbContextOptions<FlatDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Возвращает таблицу пользователей.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;
}