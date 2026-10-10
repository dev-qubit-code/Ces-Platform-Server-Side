using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;

public interface IArticleService
{
    public Task<ArticleResponse> CreateArticle(CreateArticleRequest request, CancellationToken ct = default);
    public Task UpdateArticle(Guid articleId,UpdateArticleRequest request, CancellationToken ct = default);
    public Task<PagedResult<ArticlePageResponse>> GetPagedArticles(ArticleFilter? filter, CancellationToken ct = default);
    public Task<ArticleResponse> GetArticleById(Guid articleId,CancellationToken ct);
    public Task DeleteArticle(Guid articleId, CancellationToken ct = default);
}