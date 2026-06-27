// j'importe la classe RegisterUserRequest depuis Application
using FitTrackPro.Application.Users;
// j'importe la classe User depuis Domain
using FitTrackPro.Domain.Entities;
// j'importe mon contexte de base de données
using FitTrackPro.Infrastructure.Data;
// j'importe les outils pour créer un Controller
using Microsoft.AspNetCore.Mvc;

// je déclare que ce fichier appartient au dossier Controllers de l'Api
namespace FitTrackPro.Api.Controllers;

// je dis que c'est un Controller API
[ApiController]
// l'URL de ce controller sera : /api/users
[Route("api/users")]
public class UsersController : ControllerBase
{
    // je stocke la connexion à la base de données
    private readonly FitTrackDbContext _context;

    // je reçois la connexion BDD automatiquement (injection de dépendances)
    public UsersController(FitTrackDbContext context)
    {
        _context = context;
    }

    // cet endpoint répond aux requêtes POST sur /api/users/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        // je crée un nouvel utilisateur avec les données reçues
        var user = new User
        {
            Id = Guid.NewGuid(),        // je génère un identifiant unique
            FirstName = request.FirstName,
            Email = request.Email,
            //L'utilisateur envoie son mot de passe depuis React/Postman et je stocke dans la base de donnees 
            PasswordHash = request.Password // temporaire, pas encore sécurisé
        };

        // j'ajoute l'utilisateur dans la base de données
        _context.Users.Add(user);

        // je sauvegarde vraiment en base de données
        await _context.SaveChangesAsync();

        // je retourne l'utilisateur créé en réponse JSON
        return Ok(user);
    }
}