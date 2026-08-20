using FitTrackPro.Domain.Entities;
using FitTrackPro.Application.Users;

namespace FitTrackPro.Application.Interfaces;

public interface IUserService
{
    Task<User> CreateUserAsync(RegisterUserRequest request);
}
/*
Task<User>	| Le type de retour : une tâche asynchrone qui, une fois terminée, donnera un User
CreateUserAsync	 | Le nom de la méthode
(RegisterUserRequest request) | Le paramètre attendu : un objet RegisterUserRequest
; (pas de { }) | Pas de corps de méthode — juste une signature, la promesse qu'une classe va l'implémenter

*/