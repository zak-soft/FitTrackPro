// On importe les entités du domaine (ici la classe User)
using FitTrackPro.Domain.Entities;

// On déclare dans quel "dossier logique" (namespace) se trouve ce fichier
namespace FitTrackPro.Application.Interfaces;

// On définit un contrat : toute classe qui implémente cette interface
// DOIT avoir ces 3 méthodes
public interface IUserRepository
{
    // Ajouter un utilisateur en base de données (de façon asynchrone)
    // Task = "je vais faire quelque chose, attends que ce soit fini"
    Task AddAsync(User user);

    // Chercher un utilisateur par son email
    // Task<User?> = "je vais te retourner un User... ou null si introuvable"
    // Le ? veut dire : peut être null (l'utilisateur n'existe peut-être pas)
    Task<User?> GetByEmailAsync(string email);

    // Sauvegarder les changements en base de données
    // (comme un "ctrl+S" après avoir fait des modifications)
    Task SaveChangesAsync();
}