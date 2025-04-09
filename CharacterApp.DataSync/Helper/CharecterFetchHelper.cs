using CharacterApp.Data.Model;
using CharacterApp.Data.Repository;
using CharacterApp.DataSync.Client;
using CharacterApp.DataSync.Model;

namespace CharacterApp.DataSync.Helper
{
    public class CharecterFetchHelper
    {
        private readonly ICharacterRepository _characterRepository;
        public CharecterFetchHelper(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }
        public async Task SyncDataFromApiAsync()
        {
            var client = new CharacterApiClient();
            Console.WriteLine("Fetching characters...");

            var data = await client.FetchAllCharactersAsync();

            Console.WriteLine("Received data:");

            await SyncDataToDbAsync(data);
        }

        private async Task SyncDataToDbAsync(List<CharacterResponse> responses)
        {
            await _characterRepository.ClearData();

            foreach (var response in responses)
            {
                var charecter = new Character()
                {
                    Name = response.Name,
                    Status = response.Status,
                    Species = response.Species,
                    Type = response.Type,
                    Gender = response.Gender,
                    Image = response.Image,
                    Url = response.Url,
                    Created = response.Created
                };

                var locationInfo = await _characterRepository.InsertAndGetLocationInfo(response.Location.Name, response.Location.Url);            
                charecter.Location = locationInfo;
                charecter.LocationId = locationInfo.Id;

                var originInfo = await _characterRepository.InsertAndGetLocationInfo(response.Origin.Name, response.Origin.Url);
                charecter.Origin = originInfo;
                charecter.OriginId = originInfo.Id;

                var episodeInfo = await _characterRepository.InsertAndGetEpisode(response.Episode);
                string episodeString = string.Join(",", episodeInfo);

                charecter.Episodes = episodeString;

                await _characterRepository.InsertCharacterAsync(charecter);
            }
        }
    }
}
