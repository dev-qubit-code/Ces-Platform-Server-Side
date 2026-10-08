
using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Models;

public interface IArticleRepository
{
    public Task<(int,List<Article>)> GetArticlesPageAsync(ArticleFilter? filter, CancellationToken ct = default);
    public Task<Article?> GetArticleByIdAsync(Guid articleId, CancellationToken ct = default);
    public Task<bool> AddArticleAsync(Article article, CancellationToken ct = default);
    public Task<bool> UpdateArticleAsync(CancellationToken ct = default);
    public Task<bool> DeleteArticleAsync(Article article, CancellationToken ct = default);
    public Task<int> GetArticlesCountAsync(CancellationToken ct = default);
}