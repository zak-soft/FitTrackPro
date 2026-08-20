// j'importe la classe User depuis le projet Domain
using FitTrackPro.Domain.Entities;

// j'importe Entity Framework Core pour pouvoir utiliser DbContext
using Microsoft.EntityFrameworkCore;

// je déclare que ce fichier appartient au projet Infrastructure
namespace FitTrackPro.Infrastructure.Data;

// je crée ma classe qui représente la connexion à la base de données
// elle hérite de DbContext (c'est Entity Framework qui gère tout)
public class FitTrackDbContext : DbContext
{
    // je reçois la configuration (nom de la BDD, chemin, etc.)
    // et je la passe à Entity Framework via base(options)
    public FitTrackDbContext(DbContextOptions<FitTrackDbContext> options)
        : base(options)
    {
        //il fait : Application -> Repository -> DbContext -> EF Core ->SQLite
    }

    // je dis à Entity Framework que j'ai une table "Users" dans ma BDD
    // qui contient des objets de type User
    public DbSet<User> Users { get; set; }
}