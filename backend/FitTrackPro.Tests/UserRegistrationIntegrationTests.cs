using FitTrackPro.Application.Interfaces;
using FitTrackPro.Application.Users;
using FitTrackPro.Domain.Entities;
using FitTrackPro.Infrastructure.Data;
using FitTrackPro.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FitTrackPro.Tests;

public class UserRegistrationIntegrationTests
{
    [Fact]
    public async Task CreateUser_ShouldSaveUserInDatabase()
    {
        // Arrange
        // Je crée une vraie base SQLite en mémoire.
        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        // J'ouvre la connexion SQLite.
        await connection.OpenAsync();

        // Je configure Entity Framework pour utiliser SQLite.
        var options = new DbContextOptionsBuilder<FitTrackDbContext>()
            .UseSqlite(connection)
            .Options;

        // Je crée le DbContext.
        await using var context =
            new FitTrackDbContext(options);

        // Je crée les tables dans SQLite.
        await context.Database.EnsureCreatedAsync();

        // Je crée le vrai repository.
        IUserRepository repository =
            new UserRepository(context);

        // Je crée le vrai PasswordHasher.
        IPasswordHasher<User> passwordHasher =
            new PasswordHasher<User>();

        // Je crée le vrai UserService.
        var service = new UserService(
            passwordHasher,
            repository);

        var request = new RegisterUserRequest
        {
            FirstName = "ZAKII",
            Email = "zakii@test.com",
            Password = "password123"
        };

        // Act
        // J'utilise le vrai UserService.
        var user = await service.CreateUserAsync(request);

        // Assert
        // Je vérifie que l'utilisateur a été créé.
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("ZAKII", user.FirstName);
        Assert.Equal("zakii@test.com", user.Email);

        // Je vérifie que le mot de passe n'est pas stocké en clair.
        Assert.NotEqual("password123", user.PasswordHash);

        // Je vérifie que l'utilisateur existe réellement dans SQLite.
        var savedUser =
            await repository.GetByEmailAsync("zakii@test.com");

        Assert.NotNull(savedUser);

        Assert.Equal(user.Id, savedUser.Id);
        Assert.Equal(user.Email, savedUser.Email);
    }
}