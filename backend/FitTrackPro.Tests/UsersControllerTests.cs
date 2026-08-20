using FitTrackPro.Api.Controllers;
using FitTrackPro.Application.Interfaces;
using FitTrackPro.Application.Users;
using FitTrackPro.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FitTrackPro.Tests;

public class UsersControllerTests
{
    [Fact]
    public async Task Register_ShouldReturnOk_WhenUserIsCreated()
    {
        // Arrange
        // Je prépare un faux UserService.
        var fakeUserService = new FakeUserService();

        var controller = new UsersController(fakeUserService);

        var request = new RegisterUserRequest
        {
            FirstName = "ZAKII",
            Email = "zakii@test.com",
            Password = "password123"
        };

        // Act
        // J'appelle directement la méthode du Controller.
        var result = await controller.Register(request);

        // Assert
        // Je vérifie que le Controller retourne bien HTTP 200.
        var okResult = Assert.IsType<OkObjectResult>(result);

        // Je vérifie que la réponse contient bien un UserResponse.
        var response = Assert.IsType<UserResponse>(okResult.Value);

        Assert.Equal("ZAKII", response.FirstName);
        Assert.Equal("zakii@test.com", response.Email);
    }

    [Fact]
    public async Task Register_ShouldReturnConflict_WhenEmailAlreadyExists()
    {
        // Arrange
        // Je prépare un faux UserService qui simule
        // un utilisateur avec le même email.
        var fakeUserService = new FakeUserService
        {
            EmailAlreadyExists = true
        };

        var controller = new UsersController(fakeUserService);

        var request = new RegisterUserRequest
        {
            FirstName = "ZAKII",
            Email = "zakii@test.com",
            Password = "password123"
        };

        // Act
        // J'appelle le Controller avec un email déjà utilisé.
        var result = await controller.Register(request);

        // Assert
        // Je vérifie que le Controller retourne HTTP 409 Conflict.
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);

        Assert.Equal(409, conflictResult.StatusCode);
    }
}


// Faux UserService utilisé uniquement pour les tests.
// Il permet de tester le Controller sans utiliser SQLite,
// EF Core ou le vrai UserService.
public class FakeUserService : IUserService
{
    public bool EmailAlreadyExists { get; set; }

    public Task<User> CreateUserAsync(RegisterUserRequest request)
    {
        // Je simule le comportement du vrai UserService
        // lorsqu'un email existe déjà.
        if (EmailAlreadyExists)
        {
            throw new InvalidOperationException(
                "Un utilisateur avec cet email existe déjà.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            Email = request.Email,
            PasswordHash = "fake-hash",
            CreatedAt = DateTime.UtcNow
        };

        return Task.FromResult(user);
    }
}