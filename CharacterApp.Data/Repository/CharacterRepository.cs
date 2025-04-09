using CharacterApp.Data.Context;
using CharacterApp.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace CharacterApp.Data.Repository
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _context;

        public CharacterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task ClearData()
        {
            _context.Characters.RemoveRange(_context.Characters);
            _context.Episodes.RemoveRange(_context.Episodes);
            _context.Locations.RemoveRange(_context.Locations);

            await _context.SaveChangesAsync();
        }

        public async Task InsertCharacterAsync(Character character)
        {
                character.Created = character.Created == default ? DateTime.UtcNow : character.Created;
                character.Status = string.IsNullOrEmpty(character.Status) ? "Alive" : character.Status;

                _context.Characters.Add(character);
                await _context.SaveChangesAsync();
        }

        public async Task<LocationInfo> InsertAndGetLocationInfo(string name, string url)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(x => x.Url == url);

            if (location != null)
            {
                return location;
            }

            var locationInfo = new LocationInfo { Name = name, Url = url };
            _context.Locations.Add(locationInfo);
            await _context.SaveChangesAsync();

            return locationInfo;
        }

        public async Task<List<int>> InsertAndGetEpisode(List<string> urls)
        {
            var result = new List<int>();
            foreach (var url in urls)
            {
                var episode = _context.Episodes.FirstOrDefault(x => x.Url == url);

                if (episode != null)
                {
                    result.Add(episode.Id);
                }
                else
                {
                    var episodeInfo = new Episode { Url = url };
                    _context.Episodes.Add(episodeInfo);
                    await _context.SaveChangesAsync();

                    result.Add(episodeInfo.Id); 
                }                
            }
            return result;
        }

        public async Task<List<Character>> GetCharactersAsync()
        {
            return await _context.Characters.ToListAsync();
        }

        public async Task<LocationInfo?> GetLocationInfoById(int id)
        {
            return await _context.Locations
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<string>> GetEpisodesByIds(List<int> ids)
        {
            return await _context.Episodes
                                 .Where(e => ids.Contains(e.Id)) 
                                 .Select(x => x.Url)
                                 .ToListAsync();                 
        }

        public async Task<List<LocationInfo>> GetLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        public async Task<List<Episode>> GetEpisodes()
        {
            return await _context.Episodes.ToListAsync();
        }

        public async Task<List<Character>> GetCharactersByPlanet(string planetName)
        {
            return await _context.Characters
                .Include(c => c.Location)
                .Where(c => c.Location.Name == planetName)
                .ToListAsync();
        }


    }
}