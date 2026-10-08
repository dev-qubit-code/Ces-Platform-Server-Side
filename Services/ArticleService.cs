using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.Exceptions;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.FIlters.QueryFilters;
using System.Security.Claims;
using Ces_Platform_Server_Side.Enums;

namespace Ces_Platform_Server_Side.Services;

public class ArticleService(IArticleRepository repository,ILoggerWrapper<Teacher> logger,IHttpContextAccessor accessor) : IArticleService 
{
    public async Task<ArticleResponse> CreateArticle(CreateArticleRequest request, CancellationToken ct = default)
    {
        var principal = accessor.HttpContext!.User;

        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

        var article =  Article.Create(request,username!);

        if(!await repository.AddArticleAsync(article, ct))
        {
            logger.LogError($"Error occured while adding a new article {article.Id} by {username} at {DateTime.Now}", userRole);
            
            throw new InvalidOperationException("Error occured while adding the article");
        }

        logger.LogInformation($"Create new article {article.Id} by {username} at {article.CreatedAtUtc}",userRole);

        return ArticleResponse.FromModel(article);
    } 
    
    public async Task UpdateArticle(Guid articleId,UpdateArticleRequest request, CancellationToken ct = default)
    {   
        var principal = accessor.HttpContext!.User;

        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

        var article = await repository.GetArticleByIdAsync(articleId);

        if(article is null)
        {
            logger.LogWarning($"article {articleId} not found at {DateTime.Now} requested by {username}",userRole);
            
            throw new BusinessRuleException("Article not found",StatusCodes.Status404NotFound);
        }

        if(article.IsEqual(request))
            return;
            
        article.Assign(request,username!);
 
        if(!await repository.UpdateArticleAsync(ct))
        {
            logger.LogError($"Error occured while updating the article {article.Id} at {DateTime.Now} by {username}",userRole);

            throw new InvalidOperationException("Error occured while updating the article");
        }

        logger.LogInformation($"updated article {article.Id} by {username} at {article.LastModifiedAtUtc}",userRole);

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
        var principal = accessor.HttpContext!.User;

        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

        var article = await repository.GetArticleByIdAsync(articleId);

        if(article is null)
        {
            logger.LogWarning($"article {articleId} not found at {DateTime.Now} requested by {username}",userRole);
            
            throw new BusinessRuleException("Article not found",StatusCodes.Status404NotFound);
        }

        return ArticleResponse.FromModel(article);
    } 

    public async Task DeleteArticle(Guid articleId, CancellationToken ct = default)
    {
        var principal = accessor.HttpContext!.User;

        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

        var article = await repository.GetArticleByIdAsync(articleId);

        if(article is null)
        {
            logger.LogWarning($"article {articleId} not found at {DateTime.Now} requested by {username}",userRole);
            
            throw new BusinessRuleException("Article not found",StatusCodes.Status404NotFound);
        } 

        if(!await repository.DeleteArticleAsync(article, ct))
        {
            logger.LogError($"Error occured while deleting the article {article.Id} at {DateTime.Now} by {username}",userRole);  
            
            throw new InvalidOperationException("Error occurd while deleting the article");
        } 

        logger.LogInformation($"deleted article {article.Id} by {username} at {DateTime.Now}",userRole);
    }

}
