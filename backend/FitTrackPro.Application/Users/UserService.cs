using FitTrackPro.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using FitTrackPro.Application.Interfaces;
// je déclare que ce fichier appartient à la couche Application
namespace FitTrackPro.Application.Users;
// je crée le service qui contient la logique métier des utilisateurs
public class UserService : IUserService
{
    //mon service pour fonctionner, il a besoin d'un PasswordHasher et d'un IUserRepository
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;

    public UserService(
        IPasswordHasher<User> passwordHasher,
        IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    // je crée une méthode qui fabrique un nouvel utilisateur
    // le constructeur reçoit les données du formulaire et retourne un objet User
    public async Task<User> CreateUserAsync(RegisterUserRequest request)
    {
        // je vérifie d'abord qu'aucun utilisateur n'existe déjà avec cet email
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);

        if (existingUser != null)
        {
            throw new InvalidOperationException(
                "Un utilisateur avec cet email existe déjà.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),            // je génère un identifiant unique
            FirstName = request.FirstName,  // je récupère le prénom du formulaire
            Email = request.Email,          // je récupère l'email du formulaire
            //PasswordHash = request.Password, // temporaire, pas encore sécurisé
            CreatedAt = DateTime.UtcNow     // je note la date de création maintenant
        };

        // je hache le mot de passe en clair reçu du formulaire, jamais stocké tel quel
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }
}