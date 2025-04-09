using CharacterApp.Data.Context;
using CharacterApp.Models;
using CharacterApp.Services.Interface;
using CharacterApp.Data.Repository;
using CharacterApp.Data.Model;

namespace CharacterApp.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;

        public CharacterService(ICharacterRepository characterRepository)
        {
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
                var episodeIds = c.Episodes.Split(',')           
                           .Select(id => int.Parse(id))  
                           .ToList();                   

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
                    Location = new LocationInfoViewModel() { Id = locationInfo.Id, Name = locationInfo.Name, Url = locationInfo.Url},  
                    Origin = new LocationInfoViewModel() { Id = orginInfo.Id, Name = orginInfo.Name, Url = orginInfo.Url}  
                };

                characterResult.Add(characterViewModel);
            }

            return characterResult;
        }

        public async Task InsertCharactersAsync(Character characters)
        {
            await _characterRepository.InsertCharacterAsync(characters);
        }
        public async Task<List<LocationInfo>> GetLocationInfos()
        {
            return await _characterRepository.GetLocations();
        }

        public async Task<List<Episode>> GetEpisodes()
        {
            return await _characterRepository.GetEpisodes();
        }
        public async Task<List<CharacterViewModel>> GetCharactersByPlanetAsync(string planetName)
        {
            var characters = await _characterRepository.GetCharactersByPlanet(planetName);
            return characters.Select(c => new CharacterViewModel
            {
                Name = c.Name,
                Species = c.Species,
                Gender = c.Gender,
            }).ToList();
        }
    }
}
