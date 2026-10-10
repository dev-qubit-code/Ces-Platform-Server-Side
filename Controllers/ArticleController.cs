using Asp.Versioning;
using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ces_Platform_Server_Side.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/articles")]
[ApiVersion("1.0")]
[Tags("Articles")]
[Authorize("Admin")]
public class ArticleController(IArticleService articleService) : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<ArticleResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("CreateArticle")]
    [EndpointSummary("Create article")]
    [EndpointDescription("Create article")]
    public async Task<ActionResult<ArticleResponse>> CreateArticle(CreateArticleRequest request, CancellationToken ct = default) 
    {
        var articleResponse = await articleService.CreateArticle(request, ct);

        return CreatedAtAction(nameof(GetArticleById), new { articleId = articleResponse.Id }, articleResponse);
    } 

    [HttpPut("{articleId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("UpdateArticle")]
    [EndpointSummary("Updates article")]
    [EndpointDescription("Updates article")]
    public async Task<ActionResult> UpdateArticle(Guid articleId,UpdateArticleRequest request, CancellationToken ct = default) 
    {
        await articleService.UpdateArticle(articleId,request, ct);

        return NoContent();
    } 

    [HttpGet("{articleId}")]
    [Consumes("application/json")]
    [ProducesResponseType<ArticleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetArticleById")]
    [EndpointSummary("Get article by id")]
    [EndpointDescription("Get article by id")]
    public async Task<ActionResult<ArticleResponse>> GetArticleById(Guid articleId, CancellationToken ct = default) 
    {
        var articleResponse = await articleService.GetArticleById(articleId, ct);

        return Ok(articleResponse);
    }
    
    [HttpGet]
    [Consumes("application/json")]
    [ProducesResponseType<List<ArticleResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetArticlePage")]
    [EndpointSummary("Get a page of articles")]
    [EndpointDescription("Get the page articles")]
    public async Task<ActionResult<PagedResult<ArticleResponse>>> GetArticlePage([FromQuery]ArticleFilter? filter,CancellationToken ct = default) 
    {
        var articlesPageResponse = await articleService.GetPagedArticles(filter,ct);

        return Ok(articlesPageResponse);
    }

    [HttpDelete("{articleId}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("DeleteArticle")]
    [EndpointSummary("Delete article")]
    [EndpointDescription("Delete article.")]
    public async Task<ActionResult> DeleteArticle(Guid articleId, CancellationToken ct = default) 
    {
        await articleService.DeleteArticle(articleId, ct);

        return NoContent();
    }

}