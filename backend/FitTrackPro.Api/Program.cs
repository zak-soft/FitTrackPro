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
}
