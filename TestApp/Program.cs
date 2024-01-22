using Microsoft.EntityFrameworkCore;
using TestApp.DB.Contexts;

var app = SetupContainer();

SetupApp();

app.Run();

return;

WebApplication SetupContainer()
{
    var builder = WebApplication.CreateBuilder(args);

    string? dbConnectionString =  builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<FlatDbContext>(p => p.UseSqlite(dbConnectionString));
    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    return builder.Build();
}

void SetupApp()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}