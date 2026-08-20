namespace FitTrackPro.Application.Users;

public class UserResponse
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}