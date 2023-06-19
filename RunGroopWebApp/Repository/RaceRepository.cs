using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository;

public class RaceRepository : IRaceRepository
{
    private readonly ApplicationDbContext _context;

    public RaceRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public bool Add(Race race)
    {
        _context.Add(race);
        return Save();
    }

    public bool Update(Race race)
    {
        _context.Update(race);
        return Save();
    }

    public bool Delete(Race race)
    {
        _context.Remove(race);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }

    public async Task<IEnumerable<Race>> GetAll()
    {
        return await _context.Races.ToListAsync();
    }

    public async Task<Race> GetByIdAsync(int Id)
    {
        return await _context.Races.Include(i => i.Address).FirstOrDefaultAsync(i => i.Id == Id);
    }

    public async Task<IEnumerable<Race>> GetAllRaceByCity(string City)
    {
        return await _context.Races.Where(c => c.Address.City.Contains(City)).ToListAsync();
    }
    
    
}