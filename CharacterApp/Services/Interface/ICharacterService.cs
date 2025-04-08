using CharacterApp.Data.Model;
using CharacterApp.Models;

namespace CharacterApp.Services.Interface
{
    public interface ICharacterService
    {
        Task<List<CharacterViewModel>> GetCharactersAsync();
        Task InsertCharactersAsync(Character characters);
        Task<List<LocationInfo>> GetLocationInfos();
        Task<List<Episode>> GetEpisodes();

    }
}
