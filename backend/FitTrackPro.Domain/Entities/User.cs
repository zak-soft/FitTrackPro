namespace FitTrackPro.Domain.Entities;

public class User
{
    //Id identifiant unique GUID = impossible à deviner
    public Guid Id { get; set; }
    
    //nom de l’utilisateur
    public string FirstName { get; set; }

    //Email sert pour login
    public string Email { get; set; }
    //passwordHash : jamais je stocke un mot de passe en clair, je stocke une version securisée 
    public string PasswordHash { get; set; }
    //date de création
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}