using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CollectionsAndLinq.BL.Services
{
    public class Client
    {
        private readonly string baseUrl = "https://bsa-dotnet.azurewebsites.net/api/";
        private readonly HttpClient _client;

        public Client()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }
        public Task<T> Get<T>(string url)
        {
            return Task.FromResult(
                JsonConvert.DeserializeObject<T>(
                    _client.GetAsync(url)
                    .Result.Content.ReadAsStringAsync().Result
                    ));   
        }
    }
}
