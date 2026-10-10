using Microsoft.EntityFrameworkCore;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.FIlters.QueryFilters;

public class ArticleRepository(AppDbContext context) : IArticleRepository
{
    public async Task<(int,List<Article>)> GetArticlesPageAsync(ArticleFilter? filter, CancellationToken ct = default)
    {
        IQueryable<Article> articles = context.Articles;

        List<Article> page;
        int totalCount;

        if(filter is null)
        {
            page = await articles.Take(10).ToListAsync(ct);

            totalCount = await articles.CountAsync();
            return (totalCount,page);
        }

        filter.PageSize = Math.Max(1, filter.PageSize);
        filter.Page = Math.Clamp(filter.Page, 1, 100);

        if(!string.IsNullOrWhiteSpace(filter.Search))
            articles = articles.Where(a => a.Title.Contains(filter.Search));


        totalCount = await articles.CountAsync(ct);

        page = await articles.Skip((filter.Page - 1) * filter.PageSize)
                          .Take(filter.PageSize)
                          .ToListAsync(ct);
        
        return (totalCount,page);
    }

    public async Task<Article?> GetArticleByIdAsync(Guid articleId, CancellationToken ct = default)
    {
        return await context.Articles.FirstOrDefaultAsync(a => a.Id == articleId, ct);
    }

    public async Task<bool> AddArticleAsync(Article article, CancellationToken ct = default)
    {
        context.Articles.Add(article);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateArticleAsync(CancellationToken ct = default) => await context.SaveChangesAsync(ct) > 0;

    public async Task<bool> DeleteArticleAsync(Article article, CancellationToken ct = default)
    {
        context.Articles.Remove(article);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<int> GetArticlesCountAsync(CancellationToken ct = default) => await context.Articles.CountAsync(ct);
}
