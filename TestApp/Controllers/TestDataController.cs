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
    /// <returns>Сгенерированный идентификатор человека.</returns>
    [HttpPut]
    [Route("addMan")]
    public async Task<ActionResult<Guid>> AddMan(string firstName, string lastName)
    {
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName
        };

        await _flatDbContext.Users.AddAsync(user);
        await _flatDbContext.SaveChangesAsync();

        return Ok(user.Id);
    }
}