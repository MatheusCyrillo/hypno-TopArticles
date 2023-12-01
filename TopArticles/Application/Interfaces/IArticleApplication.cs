using TopArticles.Models;

namespace TopArticles.Application.Interfaces
{
    public interface IArticleApplication
    {
        Task<IEnumerable<TopArticlesResult>> GetTopArticles(int top);
    }
}
