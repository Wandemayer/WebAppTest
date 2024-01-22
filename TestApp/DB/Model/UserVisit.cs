using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.DB.Model;

/// <summary>
/// Данные о посещении пользователя.
/// </summary>
public class UserVisit
{
    /// <summary>
    /// Возвращает идентификатор данных.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Возвращает идентификатор пользователя, к которому относятся данные.
    /// </summary>
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Возвращает время посещения.
    /// </summary>
    public DateTime VisitTime { get; set; }
}