using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interfaces;

public interface IClubRepository
{
    Task<IEnumerable<Club>> GetAll();
    Task<Club> GetByIdAsync(int Id);
    Task<Club> GetByIdAsyncNoTracking(int Id);

    Task<IEnumerable<Club>> GetClubByCity(String City);
    bool Add(Club club);
    bool Delete(Club club);
    bool Update(Club club);
    bool Save();
} 