using FitTrackPro.Domain.Entities;
// je déclare que ce fichier appartient à la couche Application
namespace FitTrackPro.Application.Users;
// je crée le service qui contient la logique métier des utilisateurs
public class UserService
{
    // je crée une méthode qui fabrique un nouvel utilisateur
    // le constructeur reçoit les données du formulaire et retourne un objet User
    public User CreateUser(RegisterUserRequest request)
    {
        return new User
        {
            Id = Guid.NewGuid(),            // je génère un identifiant unique
            FirstName = request.FirstName,  // je récupère le prénom du formulaire
            Email = request.Email,          // je récupère l'email du formulaire
            PasswordHash = request.Password, // temporaire, pas encore sécurisé
            CreatedAt = DateTime.UtcNow     // je note la date de création maintenant
        };
    }
}