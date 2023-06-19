using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interfaces;

public interface IRaceRepository
{
    bool Add(Race race);
    bool Update(Race race);
    bool Delete(Race race);
    bool Save();
    Task<IEnumerable<Race>> GetAll();
    Task<Race> GetByIdAsync(int Id);
    Task<IEnumerable<Race>> GetAllRaceByCity(string City); 
}