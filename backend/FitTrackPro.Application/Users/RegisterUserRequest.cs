using System.ComponentModel.DataAnnotations;

namespace FitTrackPro.Application.Users;

public class RegisterUserRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
}