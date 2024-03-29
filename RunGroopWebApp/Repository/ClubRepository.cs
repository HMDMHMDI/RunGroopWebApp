using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository;

public class ClubRepository : IClubRepository
{
    private readonly ApplicationDbContext _context;

    public ClubRepository(ApplicationDbContext context )
    {
        _context = context;
    }
    public async Task<IEnumerable<Club>> GetAll()
    {
        return await _context.Clubs.ToListAsync();
    }

    public async Task<Club> GetByIdAsync(int Id)
    {
        return await _context.Clubs.Include(i => i.Address).FirstOrDefaultAsync(i => i.Id == Id);
    }
    public async Task<Club> GetByIdAsyncNoTracking(int Id)
    {
        return await _context.Clubs.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == Id);
    }
    public async Task<IEnumerable<Club>> GetClubByCity(string City)
    {
        return await _context.Clubs.Where(i => i.Address.City.Contains(City)).ToListAsync();
    }

    public bool Add(Club club)
    {
        _context.Add(club);
        return Save(); 
    }

    public bool Delete(Club club)
    {
        _context.Remove(club);
        return Save();
    }

    public bool Update(Club club)
    {
        _context.Update(club);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}