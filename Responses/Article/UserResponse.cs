using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;
public class ArticleResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public DateTimeOffset Date { get; set; }
    public string Description { get; set; } = string.Empty;

    public static ArticleResponse FromModel(Article article) => new()
    {
        Id = article.Id,
        Title = article.Title,
        Description = article.Description,
        ThumbnailUrl = article.ThumbnailUrl,
        Date = article.Date,
    };

    public static IEnumerable<ArticleResponse> FromModel(IEnumerable<Article> articles) => articles.Select(FromModel);
}
