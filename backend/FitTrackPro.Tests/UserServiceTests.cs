using FitTrackPro.Application.Interfaces;
using FitTrackPro.Application.Users;
using FitTrackPro.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FitTrackPro.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task CreateUser_ShouldCreateUserWithRequestData()
    {
        // Arrange
        // Je prépare les dépendances nécessaires au UserService.

        var hasher = new PasswordHasher<User>();

        var repository = new FakeUserRepository();

        var service = new UserService(
            hasher,
            repository);

        var request = new RegisterUserRequest
        {
            FirstName = "ZAKII",
            Email = "zakii@test.com",
            Password = "password123"
        };

        // Act
        // J'exécute l'action que je veux tester.
        var user = await service.CreateUserAsync(request);

        // Assert
        // Je vérifie que l'utilisateur a bien été créé.

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("ZAKII", user.FirstName);
        Assert.Equal("zakii@test.com", user.Email);
        Assert.NotEqual("password123", user.PasswordHash);
        Assert.NotEqual(default, user.CreatedAt);

        // Je vérifie que le mot de passe en clair
        // correspond bien au hash stocké.
        var result = hasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            "password123");

        Assert.Equal(
            PasswordVerificationResult.Success,
            result);

        // Je vérifie également que le UserService
        // a bien demandé au repository de sauvegarder l'utilisateur.
        Assert.NotNull(repository.SavedUser);
        Assert.Equal(user.Id, repository.SavedUser.Id);
    }
    [Fact]
    public async Task CreateUser_ShouldNotAllowDuplicateEmail()
    {
        // Arrange
        var hasher = new PasswordHasher<User>();
        var repository = new FakeUserRepository();

        var service = new UserService(
            hasher,
            repository);

        var request = new RegisterUserRequest
        {
            FirstName = "ZAKII",
            Email = "zakii@test.com",
            Password = "password123"
        };

        // Je crée le premier utilisateur.
        await service.CreateUserAsync(request);

        // Act + Assert
        // Je tente de créer un deuxième utilisateur
        // avec le même email.
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateUserAsync(request));

        Assert.Equal(
            "Un utilisateur avec cet email existe déjà.",
            exception.Message);
    }

}
// Faux repository utilisé uniquement pour le test.
// Il permet de tester UserService sans utiliser SQLite.
public class FakeUserRepository : IUserRepository {
    public User? SavedUser { get; private set; }

    public Task AddAsync(User user)
    {
        SavedUser = user;

        return Task.CompletedTask;
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        if (SavedUser != null && SavedUser.Email == email)
        {
            return Task.FromResult<User?>(SavedUser);
        }

        return Task.FromResult<User?>(null);
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}


/*
Arrange (préparer)

Tu mets en place tout ce dont le test a besoin : créer les objets, définir les données d'entrée, configurer les mocks/dépendances.

Act (agir)

Tu exécutes une seule action — celle que tu veux réellement tester (appeler la méthode, la fonction).

*/