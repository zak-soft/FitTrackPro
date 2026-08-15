namespace FitTrackPro.Application.Users;
//registerUser request ne fait rien de particulier il  reçoit juste les données : 
//C'est juste un conteneur de données — il transporte ce que l'utilisateur a tapé dans le formulaire. Il ne fait rien, il contient juste des infos.
//Userservice : transforme en vrai User
public class RegisterUserRequest
{
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}