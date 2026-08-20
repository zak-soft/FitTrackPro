using FitTrackPro.Application.Users;
using FitTrackPro.Domain.Entities;
using FitTrackPro.Infrastructure.Data;
using FitTrackPro.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FitTrackPro.Tests;

public class UserServiceIntegrationTests
{
    [Fact]
    public async Task CreateUser_ShouldSaveUserToDatabase()
    {
        // Arrange

        // Je crée une connexion SQLite en mémoire.
        // La base sera créée uniquement pour la durée du test.
        await using var connection = new SqliteConnection("Data Source=:memory:");//SQLite : je crée alors une base temporaire en mémoire.
        //SINON je peux créer un fichier db pour voir 

        await connection.OpenAsync();

        // Je configure Entity Framework pour utiliser SQLite.
        var options = new DbContextOptionsBuilder<FitTrackDbContext>()
            .UseSqlite(connection)
            .Options;

        // Je crée le vrai DbContext.
        await using var context = new FitTrackDbContext(options);

        // Je crée les tables de la base de données.
        await context.Database.EnsureCreatedAsync();

        // J'utilise le vrai Repository.
        var repository = new UserRepository(context);

        // J'utilise le vrai PasswordHasher.
        var hasher = new PasswordHasher<User>();

        // Je crée le vrai UserService.
        var service = new UserService(
            hasher,
            repository);

        var request = new RegisterUserRequest
        {
            FirstName = "ZAKII",
            Email = "integration@test.com",
            Password = "password123"
        };

        // Act

        // Je demande au vrai UserService de créer l'utilisateur.
        var user = await service.CreateUserAsync(request);

        // Assert

        // Je vérifie que l'utilisateur a bien été créé.
        Assert.NotEqual(Guid.Empty, user.Id);

        // Je relis l'utilisateur directement depuis la vraie base SQLite.
        var savedUser = await context.Users
            .FirstOrDefaultAsync(u => u.Email == "integration@test.com");

        // Je vérifie qu'il existe réellement dans la base.
        Assert.NotNull(savedUser);

        Assert.Equal("ZAKII", savedUser.FirstName);
        Assert.Equal("integration@test.com", savedUser.Email);

        // Je vérifie que le mot de passe n'est pas stocké en clair.
        Assert.NotEqual("password123", savedUser.PasswordHash);
    }
}