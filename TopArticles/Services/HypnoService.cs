using Newtonsoft.Json;
using TopArticles.Models;
using TopArticles.Services.Interfaces;

namespace TopArticles.Services
{
    public class HypnoService : IHypnoService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;

        public HypnoService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient("hypnobox");
        }
        public async Task<Article?> GetArticlesByPage(int page)
        {
          
            var response = await _client.GetAsync($"/teste/api/articles?page={page}");

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Article?>(await response.Content.ReadAsStringAsync());
            else
                return null;
        }
    }
}
