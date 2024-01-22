using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApp.DB.Contexts;
using TestApp.DB.Model;

namespace TestApp.Controllers;

/// <summary>
/// Контроллер для редактирования тестовых данных.
/// </summary>
[Route("[controller]")]
[ApiController]
public class TestDataController(FlatDbContext _flatDbContext) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех людей.
    /// </summary>
    /// <returns>Коллекция всех людей в базе данных.</returns>
    [HttpGet]
    [Route("getPeople")]
    public async Task<ActionResult<IReadOnlyCollection<User>>> GetAllPeople()
    {
        var people = await _flatDbContext.Users.ToListAsync();

        return Ok(people);
    }

    /// <summary>
    /// Добавляет человека в базу данных.
    /// </summary>
    /// <param name="firstName">Имя человека.</param>
    /// <param name="lastName">Фамилия человека.</param>
    /// <returns>Построенная модель человека.</returns>
    [HttpPut]
    [Route("addMan")]
    public async Task<ActionResult<User>> AddMan(string firstName, string lastName)
    {
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName
        };

        await _flatDbContext.Users.AddAsync(user);
        await _flatDbContext.SaveChangesAsync();

        return Ok(user);
    }

    /// <summary>
    /// Возвращает список всех посещений человека.
    /// </summary>
    /// <param name="userId">Идентификатор человека.</param>
    /// <returns>Коллекция посещений человека.</returns>
    [HttpGet]
    [Route("getVisits")]
    public async Task<ActionResult<IReadOnlyCollection<UserVisit>>> GetUserVisits(Guid userId)
    {
        var user = await _flatDbContext.Users.FindAsync(userId);

        if (user is null)
        {
            return BadRequest(string.Format(Messages.NotFound, userId));
        }

        await _flatDbContext.Entry(user).Collection(p => p.UserVisits).LoadAsync();

        return Ok(user.UserVisits.ToList());
    }

    /// <summary>
    /// Добавляет запись о посещении человека.
    /// </summary>
    /// <param name="userId">Идентификатор человека.</param>
    /// <param name="visitTime">Время посещения.</param>
    /// <returns>Построенная модель данных посещения.</returns>
    [HttpPut]
    [Route("addVisit")]
    public async Task<ActionResult<UserVisit>> AddUserVisit(Guid userId, DateTime visitTime)
    {
        var user = await _flatDbContext.Users.FindAsync(userId);
        
        if (user is null)
        {
            return BadRequest(string.Format(Messages.NotFound, userId));
        }

        var visit = new UserVisit
        {
            VisitTime = visitTime
        };
        
        user.UserVisits.Add(visit);
        await _flatDbContext.SaveChangesAsync();

        return Ok(visit);
    }
}