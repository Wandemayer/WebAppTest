using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.DB.Model;

/// <summary>
/// Модель запроса сбора статистических данных.
/// </summary>
public class StatisticQuery
{
    /// <summary>
    /// Возвращает идентификатор запроса статистики.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Возвращает идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Возвращает идентификатор данных статистики.
    /// </summary>
    public Guid DataId { get; set; }

    /// <summary>
    /// Возвращает данные сбора статистики.
    /// </summary>
    public StatisticData Data { get; set; } = null!;
    
    /// <summary>
    /// Возвращает время начала выполнения запроса.
    /// </summary>
    public DateTime ExecuteStartTime { get; set; }
}