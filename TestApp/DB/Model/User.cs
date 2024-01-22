using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.DB.Model;

/// <summary>
/// Модель пользователя.
/// </summary>
public class User
{
    /// <summary>
    /// Возвращает идентификатор пользователя.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Возвращает имя пользователя.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Возвращает Фамилию пользователя.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Возвращает коллекцию данных о посещениях пользователя.
    /// </summary>
    public virtual ICollection<UserVisit> UserVisits { get; set; } = new List<UserVisit>();
}