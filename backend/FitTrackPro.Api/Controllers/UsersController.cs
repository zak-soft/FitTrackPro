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
    //Je déclare une variable _context qui me permet de communiquer avec la base de données depuis n'importe quelle méthode de mon Controller. 
    private readonly FitTrackDbContext _context;

    //je déclare une variable que je peux call depuis n'importe quelle methode 
    private readonly UserService _userService;

    // le constructeur reçoit automatiquement 2 choses au démarrage (injection de dépendances) :
    // - la connexion à la base de données
    // - le service qui contient la logique métier
    public UsersController(FitTrackDbContext context, UserService userService)
    {
        // je stocke la connexion BDD dans ma variable privée
        _context = context;
        // je stocke le service utilisateur dans ma variable privée
        _userService = userService;
    }

    // cet endpoint répond aux requêtes POST sur /api/users/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        // Le Controller ne sait plus comment créer un utilisateur.
        //Il dit simplement : "UserService, crée-moi un utilisateur."
        var user = _userService.CreateUser(request);

        // j'ajoute l'utilisateur dans la base de données
        _context.Users.Add(user);

        // je sauvegarde vraiment en base de données
        await _context.SaveChangesAsync();

        // je retourne l'utilisateur créé en réponse JSON
        return Ok(user);
    }
}