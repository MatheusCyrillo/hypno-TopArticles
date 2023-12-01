using TopArticles.Models;

namespace TopArticles.Services.Interfaces
{
    public interface IHypnoService
    {
        Task<Article?> GetArticlesByPage(int page);
    }
}
