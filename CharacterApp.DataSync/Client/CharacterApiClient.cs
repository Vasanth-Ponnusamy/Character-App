using CharacterApp.DataSync.Model;
using System.Text.Json;

namespace CharacterApp.DataSync.Client
{
    public class CharacterApiClient
    {
        public async Task<List<CharacterResponse>> FetchAllCharactersAsync()
        {
            var allCharacters = new List<CharacterResponse>();
            var url = Constants.ApiUrl;
            using var httpClient = new HttpClient();

            while (!string.IsNullOrEmpty(url))
            {
                var response = await httpClient.GetStringAsync(url);
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (apiResponse?.Results != null)
                {
                    allCharacters.AddRange(apiResponse.Results);
                }

                url = apiResponse?.Info?.Next;
            }

            return allCharacters.Where(x => x.Status == Constants.AliveStatus).ToList();
        }
    }
}
