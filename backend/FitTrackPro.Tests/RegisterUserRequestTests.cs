using FitTrackPro.Application.Users;
using System.ComponentModel.DataAnnotations;

namespace FitTrackPro.Tests;

public class RegisterUserRequestTests
{
    [Fact]
    public void ValidRequest_ShouldPassValidation()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "Zakaria",
            Email = "zakaria@test.com",
            Password = "password123"
        };

        // Act
        var results = Validate(request);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void InvalidEmail_ShouldFailValidation()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "Zakaria",
            Email = "email-invalide",
            Password = "password123"
        };

        // Act
        var results = Validate(request);

        // Assert
        Assert.Contains(results, r =>
            r.MemberNames.Contains(nameof(RegisterUserRequest.Email)));
    }

    [Fact]
    public void ShortPassword_ShouldFailValidation()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "Zakaria",
            Email = "zakaria@test.com",
            Password = "123"
        };

        // Act
        var results = Validate(request);

        // Assert
        Assert.Contains(results, r =>
            r.MemberNames.Contains(nameof(RegisterUserRequest.Password)));
    }

    [Fact]
    public void EmptyFirstName_ShouldFailValidation()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "",
            Email = "zakaria@test.com",
            Password = "password123"
        };

        // Act
        var results = Validate(request);

        // Assert
        Assert.Contains(results, r =>
            r.MemberNames.Contains(nameof(RegisterUserRequest.FirstName)));
    }

    // Méthode utilitaire utilisée par les différents tests.
    // Elle exécute les DataAnnotations présentes sur RegisterUserRequest.
    private static List<ValidationResult> Validate(
        RegisterUserRequest request)
    {
        var context = new ValidationContext(request);

        var results = new List<ValidationResult>();

        Validator.TryValidateObject(
            request,
            context,
            results,
            validateAllProperties: true);

        return results;
    }
}