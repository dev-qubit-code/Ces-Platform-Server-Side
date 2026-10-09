namespace Ces_Platform_Server_Side.Requests;
public class CreateArticleRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    // public required IFormFile Thumbnail { get; set; }
    public DateTimeOffset Date { get; set; }
}
