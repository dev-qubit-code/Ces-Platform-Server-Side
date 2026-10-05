using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.Exceptions;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.FIlters.QueryFilters;

namespace Ces_Platform_Server_Side.Services;

public class ArticleService(IArticleRepository repository) : IArticleService 
{
    public async Task<ArticleResponse> CreateArticle(CreateArticleRequest request, CancellationToken ct = default)
    {
        
        var article =  Article.Create(request,"testName");

        if(!await repository.AddArticleAsync(article,ct))
            throw new InvalidOperationException("Error occured while adding the article");

        return ArticleResponse.FromModel(article);
    } 
    
    public async Task UpdateArticle(Guid articleId,UpdateArticleRequest request, CancellationToken ct = default)
    {   
        var article = await repository.GetArticleByIdAsync(articleId);

        if(article is null)
            throw new BusinessRuleException("Article not found",StatusCodes.Status404NotFound);

        if(article.IsEqual(request))
            return;
            
        article.Assign(request,"testName");
 
        if(!await repository.UpdateArticleAsync(ct))
            throw new InvalidOperationException("Error occured while updating the article");
    } 

    public async Task<PagedResult<ArticlePageResponse>> GetPagedArticles(ArticleFilter? filter, CancellationToken ct = default)
    {
         
        
        (int totalCount,var articles) = await repository.GetArticlesPageAsync(filter, ct);

        filter ??= new();

        if(articles is null || !articles.Any()) 
            return PagedResult<ArticlePageResponse>.Create(
            [],
            totalCount,
            filter.Page,
            filter.PageSize);

        var pagedResult = PagedResult<ArticlePageResponse>.Create(
            ArticlePageResponse.FromModels(articles),
            totalCount,
            filter.Page,
            filter.PageSize);

        return pagedResult;
    }
    public async Task<ArticleResponse> GetArticleById(Guid articleId,CancellationToken ct)
    {
        var article = await repository.GetArticleByIdAsync(articleId,ct) ?? throw new BusinessRuleException("Article not found",StatusCodes.Status404NotFound); 

        return ArticleResponse.FromModel(article);
    } 

    public async Task DeleteArticle(Guid articleId, CancellationToken ct = default)
    {
        var article = await repository.GetArticleByIdAsync(articleId,ct) ?? throw new BusinessRuleException("Article not found",StatusCodes.Status404NotFound); 

        if(!await repository.DeleteArticleAsync(article, ct)) 
            throw new InvalidOperationException("Error occurd while deleting the article");
    }

}
