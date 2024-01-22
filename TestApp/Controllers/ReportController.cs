using Microsoft.AspNetCore.Mvc;
using TestApp.DB.Contexts;
using TestApp.DB.Model;

namespace TestApp.Controllers;

/// <summary>
/// Контролер получения статистических данных.
/// </summary>
[Route("report")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly FlatDbContext _flatDbContext;
    private readonly int _queryExecuteTime;

    public ReportController(IConfiguration configuration, FlatDbContext flatDbContext)
    {
        _flatDbContext = flatDbContext;

        const int DEFAULT_QUERY_EXECUTE_TIME = 60000;
        _queryExecuteTime = DEFAULT_QUERY_EXECUTE_TIME;

        if (int.TryParse(configuration["QueryExecuteTime"], out int configQueryExecuteTime))
        {
            _queryExecuteTime = configQueryExecuteTime;
        }
    }

    /// <summary>
    /// Выполняет запрос на формирование статистических данных пользователя за указанный срок.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="from">Дата начала формирования статистики.</param>
    /// <param name="to">Дата окончания формирования статистики</param>
    /// <returns>Идентификатор запроса.</returns>
    [HttpPost]
    [Route("user_statics")]
    public async Task<ActionResult<Guid>> PostUserStatisticsAsync(Guid userId, DateTime from, DateTime to)
    {
        var user = await _flatDbContext.Users.FindAsync(userId);

        if (user is null)
        {
            return BadRequest(string.Format(Messages.NotFound, userId));
        }

        await _flatDbContext.Entry(user).Collection(p => p.UserVisits).LoadAsync();

        int visitsCount = user.UserVisits.Count(p => p.VisitTime >= from && p.VisitTime <= to);

        var statData = new StatisticData
        {
            From = from,
            To = to,
            VisitsCount = visitsCount
        };

        await _flatDbContext.Statistic.AddAsync(statData);
        await _flatDbContext.SaveChangesAsync();

        var query = new StatisticQuery
        {
            UserId = userId,
            DataId = statData.Id,
            Data = statData,
            ExecuteStartTime = DateTime.Now
        };

        await _flatDbContext.Queries.AddAsync(query);
        await _flatDbContext.SaveChangesAsync();

        return Ok(query.Id);
    }

    /// <summary>
    /// Возвращает результат выполнения запроса сбора данных.
    /// </summary>
    /// <param name="queryId">Идентификатор запроса.</param>
    /// <returns>Результат выполнения сбора данных.</returns>
    [HttpGet]
    [Route("info")]
    public async Task<ActionResult<QueryExecuteData>> GetQueryInfoAsync(Guid queryId)
    {
        var query = await _flatDbContext.Queries.FindAsync(queryId);

        if (query is null)
        {
            return BadRequest(string.Format(Messages.NotFound, queryId));
        }

        var currentTime = DateTime.Now;
        double queryExecuteTime = currentTime.Subtract(query.ExecuteStartTime).TotalMilliseconds;
        int percent = (int)Math.Min(queryExecuteTime / _queryExecuteTime * 100, 100);

        StatisticData? data = null;

        if (percent >= 100)
        {
            await _flatDbContext.Entry(query).Reference(p => p.Data).LoadAsync();

            data = query.Data;
        }

        var queryExecuteResult = new QueryExecuteData
        {
            QueryId = queryId,
            Percent = percent,
            StatisticData = data
        };

        return Ok(queryExecuteResult);
    }
}

/// <summary>
/// Модель запроса данных.
/// </summary>
public class QueryExecuteData
{
    /// <summary>
    /// Возвращает идентификатор запроса.
    /// </summary>
    public required Guid QueryId { get; init; }

    /// <summary>
    /// Возвращает процент выполнения.
    /// </summary>
    public required int Percent { get; init; }

    /// <summary>
    /// Возвращает данные сбора статистики; <see langword="null"/>, если данные еще не собраны.
    /// </summary>
    public StatisticData? StatisticData { get; init; }
}