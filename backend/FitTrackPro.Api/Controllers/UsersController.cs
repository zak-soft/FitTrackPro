// j'importe la classe RegisterUserRequest depuis Application
using FitTrackPro.Application.Users;
// j'importe les outils pour créer un Controller
using Microsoft.AspNetCore.Mvc;
using FitTrackPro.Application.Interfaces;
// je déclare que ce fichier appartient au dossier Controllers de l'Api
namespace FitTrackPro.Api.Controllers;

// je dis que c'est un Controller API
[ApiController]

// l'URL de ce controller sera : /api/users
[Route("api/users")]
public class UsersController : ControllerBase
{
    // je déclare une variable que je peux appeler depuis n'importe quelle méthode
    // Le Controller utilise le UserService pour demander la création d'un utilisateur.
    // Il ne communique plus directement avec la base de données.
    private readonly IUserService _userService;

    // le constructeur reçoit automatiquement le UserService grâce à l'injection de dépendances
    public UsersController(IUserService userService)
    {
        // je stocke le service utilisateur dans ma variable privée
        _userService = userService;
    }

    // cet endpoint répond aux requêtes POST sur /api/users/register
   [HttpPost("register")]
// cette méthode répond aux requêtes HTTP POST envoyées sur /register
public async Task<IActionResult> Register(RegisterUserRequest request)
{
    // Le Controller délègue la création au UserService.
    try
    {
        // j'appelle le service pour créer l'utilisateur avec les données reçues
        var user = await _userService.CreateUserAsync(request);

        // Je construis une réponse qui ne contient pas le PasswordHash.
        // je crée un nouvel objet "propre" à renvoyer au client,
        // pour ne jamais exposer le mot de passe haché dans la réponse
        var response = new UserResponse
        {
            Id = user.Id,               // je recopie l'id généré
            FirstName = user.FirstName, // je recopie le prénom
            Email = user.Email,         // je recopie l'email
            CreatedAt = user.CreatedAt  // je recopie la date de création
        };

        // Je retourne l'utilisateur créé.
        // Ok() renvoie un code HTTP 200 : tout s'est bien passé,
        // avec "response" comme contenu de la réponse
        return Ok(response);
    }
    catch (InvalidOperationException ex)
    {
        // L'utilisateur existe déjà.
        // je récupère l'exception envoyée par UserService
        // quand un email est déjà utilisé

        // 409 Conflict signifie que la requête est valide,
        // mais qu'elle entre en conflit avec l'état actuel de la ressource.
        // je renvoie ce code + le message d'erreur dans un petit objet JSON
        return Conflict(new
        {
            message = ex.Message
        });
    }
}
}