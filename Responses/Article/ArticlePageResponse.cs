using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;

public class ArticlePageResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public DateTimeOffset Date { get; set; }

    public static ArticlePageResponse FromModel(Article article) => new()
    {
        Id = article.Id,
        Title = article.Title,
        ThumbnailUrl = article.ThumbnailUrl,
        Date = article.Date,
    };

    public static IEnumerable<ArticlePageResponse> FromModels(IEnumerable<Article> articles) => articles.Select(FromModel);
}