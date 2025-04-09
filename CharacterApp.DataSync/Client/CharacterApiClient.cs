using CharacterApp.DataSync.Model;
using System.Net.Security;
using System.Security.Authentication;
using System.Text.Json;

namespace CharacterApp.DataSync.Client
{
    public class CharacterApiClient
    {
        //public async Task<List<CharacterResponse>> FetchAllCharactersAsync()
        //{
        //    var allCharacters = new List<CharacterResponse>();
        //    var url = Constants.ApiUrl;
        //    using var httpClient = new HttpClient();

        //    while (!string.IsNullOrEmpty(url))
        //    {
        //        var response = await httpClient.GetStringAsync(url);
        //        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(response, new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        });

        //        if (apiResponse?.Results != null)
        //        {
        //            allCharacters.AddRange(apiResponse.Results);
        //        }

        //        url = apiResponse?.Info?.Next;
        //    }

        //    return allCharacters.Where(x => x.Status == Constants.AliveStatus).ToList();
        //}

        public async Task<List<CharacterResponse>> FetchAllCharactersAsync()
        {
            var allCharacters = new List<CharacterResponse>();
            var handler = new SocketsHttpHandler
            {
                SslOptions = new SslClientAuthenticationOptions
                {
                    EnabledSslProtocols = SslProtocols.Tls12
                }
            };
            using var httpClient = new HttpClient(handler);
            var initialUrl = Constants.ApiUrl;

            var firstResponse = await httpClient.GetStringAsync(initialUrl);
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(firstResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (apiResponse == null || apiResponse.Results == null || apiResponse.Info?.Pages == null)
            {
                return new List<CharacterResponse>();
            }


            allCharacters.AddRange(apiResponse.Results);

            var totalPages = apiResponse.Info.Pages;

            var tasks = new List<Task<string>>();
            for (int i = 2; i <= totalPages; i++)
            {
                var pageUrl = $"{Constants.ApiUrl}?page={i}";
                tasks.Add(httpClient.GetStringAsync(pageUrl));
            }

            var responses = await Task.WhenAll(tasks);

            foreach (var response in responses)
            {
                var pageResponse = JsonSerializer.Deserialize<ApiResponse>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (pageResponse?.Results != null)
                {
                    allCharacters.AddRange(pageResponse.Results);
                }
            }

            return allCharacters.Where(x => x.Status == Constants.AliveStatus).ToList();
        }

    }
}
