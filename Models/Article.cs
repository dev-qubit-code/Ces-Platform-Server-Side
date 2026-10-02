
using Ces_Platform_Server_Side.Requests;

namespace Ces_Platform_Server_Side.Models;
// اخبار
public class Article:AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public DateTimeOffset Date { get; set; }
   
    public Article() { }

    public Article(string title, string description, DateTimeOffset date,string createdBy):base(createdBy)
    {
        Title = title;
        Description = description;
        Date = date;
    }

    public static Article Create(CreateArticleRequest request, string createdBy) 
        => new Article(request.Title, request.Description, request.Date, createdBy);

    public void Assign(UpdateArticleRequest request, string lastModifiedBy)
    {
        Title = request.Title;
        Description = request.Description;
        Date = request.Date;

        LastModifiedAtUtc = DateTimeOffset.UtcNow;
        LastModifiedBy = lastModifiedBy;
    } 

    public bool IsEqual(UpdateArticleRequest request)
    {
        return Title == request.Title 
            && Description == request.Description  
            && Date == request.Date;
    }
}
