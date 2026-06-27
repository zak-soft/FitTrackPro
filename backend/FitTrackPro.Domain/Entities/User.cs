namespace FitTrackPro.Domain.Entities;

public class User
{
    //Id identifiant unique GUID = impossible à deviner
    public Guid Id { get; set; }
    
    //nom de l’utilisateur
    //string.Empty; => non null = " " 
    public string FirstName { get; set; } = string.Empty;

    //Email sert pour login
    public string Email { get; set; } = string.Empty;
    //passwordHash : jamais je stocke un mot de passe en clair, je stocke une version securisée 
    public string PasswordHash { get; set; } = string.Empty;
    //date de création
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}