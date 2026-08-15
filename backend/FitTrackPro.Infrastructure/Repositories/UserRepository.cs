using FitTrackPro.Application.Interfaces;
using FitTrackPro.Domain.Entities;
using FitTrackPro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackPro.Infrastructure.Repositories;
//"Je promets d'implémenter toutes les méthodes définies dans l'interface."
public class UserRepository : IUserRepository
{
    //Pourquoi injecter le DbContext ? car c'est lui qui sait parler SQlite
    private readonly FitTrackDbContext _context;

    public UserRepository(FitTrackDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        //public DbSet<User> Users { get; set; } : representte la table user de la base de données
        //Signifie simplement : "Prépare l'insertion d'une nouvelle ligne dans la table Users." 
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        //Fait une requête SQL du type : SELECT *, FROM Users, WHERE Email = @email, LIMIT 1;
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task SaveChangesAsync()
    {
        //je les envoie réellement à SQLite.
        await _context.SaveChangesAsync();
    }
}