// j'importe mon contexte de base de données
using FitTrackPro.Infrastructure.Data;

// j'importe Entity Framework Core
using Microsoft.EntityFrameworkCore;
using FitTrackPro.Application.Users;
using FitTrackPro.Application.Interfaces;
using FitTrackPro.Infrastructure.Repositories;

// je démarre la configuration de mon application
var builder = WebApplication.CreateBuilder(args);

// j'ajoute la base de données SQLite à mon application
// "fittrack.db" = le nom du fichier qui sera créé sur le disque
builder.Services.AddDbContext<FitTrackDbContext>(options =>
    options.UseSqlite("Data Source=fittrack.db"));
//Pourquoi AddScoped? Les durées de vie des services sont importantes :Transient : nouvelle instance à chaque utilisation.
//Scoped : une instance par requête HTTP. ✅ C'est le choix recommandé pour DbContext et les repositories.
//Singleton : une seule instance pendant toute la durée de vie de l'application.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

// j'active les Controllers (mes endpoints seront dans des fichiers séparés)
builder.Services.AddControllers();
//je dis à .NET : "UserService existe, sache le créer et l'injecter automatiquement"
// une instance sera créée par requête HTTP
builder.Services.AddScoped<UserService>();

// j'active Swagger pour pouvoir tester mon API dans le navigateur
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// je construis l'application avec tous les services configurés au-dessus
var app = builder.Build();

// j'active l'interface Swagger (accessible sur /swagger)
app.UseSwagger();
app.UseSwaggerUI();

// je force les requêtes HTTP à passer en HTTPS
app.UseHttpsRedirection();

// je branche mes Controllers pour qu'ils reçoivent les requêtes
app.MapControllers();

// je lance le serveur — l'application tourne et attend des requêtes
app.Run();

/* mon code de test au tout debut 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Dans les services : ici je peux ajouter les services là ou ou mon back tourne 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //ensuite quand je rajoute le service que je veux dans le builder je rajoute ceci dans la pipeline 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// ça c'est juste un exemple 
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// Test endpoint pour voir si User marche
app.MapGet("/test-user", () =>
{
    //c'est la ou j'ai appellé ma variable user avec son emplacement exact
    var user = new FitTrackPro.Domain.Entities.User
    {
        Id = Guid.NewGuid(),
        FirstName = "Zakaria",
        Email = "zak@fittrack.com",
        PasswordHash = "hashed_password_123"
    };

    return user;
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}*/
