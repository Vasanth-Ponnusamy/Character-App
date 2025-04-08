using CharacterApp.Data.Context;
using CharacterApp.Models;
using CharacterApp.Services.Interface;
using CharacterApp.Data.Repository;
using CharacterApp.Data.Model;

namespace CharacterApp.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly AppDbContext _context;
        private readonly ICharacterRepository _characterRepository;

        public CharacterService(AppDbContext context, ICharacterRepository characterRepository)
        {
            _context = context;
            _characterRepository = characterRepository;

        }
        public async Task<List<CharacterViewModel>> GetCharactersAsync()
        {
            var characters = await _characterRepository.GetCharactersAsync();

            var characterResult = new List<CharacterViewModel>();

            foreach (var c in characters)
            {
                var locationInfo = await _characterRepository.GetLocationInfoById(c.LocationId);
                var orginInfo = await _characterRepository.GetLocationInfoById(c.OriginId);
                var episodeIds = c.Episodes.Split(',')           // Split the comma-separated string
                           .Select(id => int.Parse(id))  // Convert each part to an integer
                           .ToList();                    // Create a list of integers

                var episodes = await _characterRepository.GetEpisodesByIds(episodeIds);

                var characterViewModel = new CharacterViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Status = c.Status,
                    Species = c.Species,
                    Type = c.Type,
                    Gender = c.Gender,
                    OriginId = c.OriginId,
                    LocationId = c.LocationId,
                    Image = c.Image,
                    Episodes = episodes,  
                    Url = c.Url,
                    Created = c.Created,
                    Location = new LocationInfoViewModel() { Id = locationInfo.Id, Name = locationInfo.Name, Url = locationInfo.Url},  // Assigned the Location info fetched asynchronously
                    Origin = new LocationInfoViewModel() { Id = orginInfo.Id, Name = orginInfo.Name, Url = orginInfo.Url}  // Assigned the Location info fetched asynchronously
                };

                characterResult.Add(characterViewModel);
            }

            return characterResult;
        }

        //public async Task<LocationInfo> GetLocationInfo(int id)
        //{

        //}



        public Task InsertCharactersAsync(List<CharacterViewModel> characters)
        {
            throw new NotImplementedException();
        }
    }
}
