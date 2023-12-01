using Newtonsoft.Json;
using TopArticles.Models;

namespace TopArticles.Services
{
    public class HypnoService
    {
        public async Task<Article> GetArticle(int page)
        {
            //HttpFactory
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"http://hypnocore.api.hypnobox.com.br");

            var response = await httpClient.GetAsync($"/teste/api/articles?page={page}");

            return JsonConvert.DeserializeObject<Article>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
