using CharacterApp.Data.Context;
using CharacterApp.Data.Model;

namespace CharacterApp.Data.Repository
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _context;

        public CharacterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertCharacterAsync(Character character)
        {
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
        }

        public async Task<LocationInfo> InsertAndGetLocationInfo(string name, string url)
        {
            var location = _context.Locations.FirstOrDefault(x => x.Url == url);

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
    }
}