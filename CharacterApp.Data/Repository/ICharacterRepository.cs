using CharacterApp.Data.Model;

namespace CharacterApp.Data.Repository
{
    public interface ICharacterRepository
    {
        Task InsertCharacterAsync(Character character);
        Task<LocationInfo> InsertAndGetLocationInfo(string name, string url);
        Task<List<int>> InsertAndGetEpisode(List<string> urls);
    }
}
