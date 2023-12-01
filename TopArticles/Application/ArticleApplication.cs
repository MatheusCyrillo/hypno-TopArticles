using TopArticles.Application.Interfaces;
using TopArticles.CustomException;
using TopArticles.Models;
using TopArticles.Services;
using TopArticles.Services.Interfaces;

namespace TopArticles.Application
{
    public class ArticleApplication : IArticleApplication
    {
        private readonly IHypnoService _hypnoService;
        public ArticleApplication(IHypnoService hypnoService)
        {
            _hypnoService = hypnoService;
        }

        public async Task<IEnumerable<TopArticlesResult>> GetTopArticles(int top)
        {
            var articles = await _hypnoService.GetArticlesByPage(1);

            if (articles == null)
                throw new BusinessRuleException("Não foi possível obter os artigos da API da Hypnobox.");

            var allArticles = articles.Data;
            List<Task<Article?>> taskArticles = new List<Task<Article?>>();

            for (int i = 2; i <= articles.TotalPages; i++)
            {
                taskArticles.Add(_hypnoService.GetArticlesByPage(i));
            }

            var articleList = await Task.WhenAll(taskArticles);

            foreach (var item in articleList)
            {
                if (item != null)
                    allArticles.AddRange(item.Data);
            }

            var filteredArticles = allArticles.Where(a => !(string.IsNullOrEmpty(a.Title) && string.IsNullOrEmpty(a.StoryTitle)));

            filteredArticles = allArticles.Where(a => a.NumComments != null);

            var orderedArticles = filteredArticles.OrderByDescending(a => a.NumComments).ThenBy(a => a.ArticleTitle, StringComparer.OrdinalIgnoreCase);

            return orderedArticles.Select(o => new TopArticlesResult { Title = o.ArticleTitle, NumComments = o.NumComments }).Take(top);
        }
    }
}
