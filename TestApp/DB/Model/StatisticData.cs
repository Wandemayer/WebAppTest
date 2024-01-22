using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.DB.Model;

/// <summary>
/// Определяет статистические данные пользователя.
/// </summary>
public class StatisticData
{
    /// <summary>
    /// Возвращает идентификатор данных.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Возвращает дату, с которой начинается данные статистики.
    /// </summary>
    public DateTime From { get; set; }
    
    /// <summary>
    /// Возвращает дату которой оканчивается данные статистики.
    /// </summary>
    public DateTime To { get; set; }
    
    /// <summary>
    /// Возвращает количество посещений пользователя за промежуток <see cref="From"/> - <see cref="To"/>.
    /// </summary>
    public int VisitsCount { get; set; }
}