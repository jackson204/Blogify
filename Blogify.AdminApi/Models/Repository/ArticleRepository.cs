using Blogify.AdminApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Blogify.AdminApi.Models.Repository;

public class ArticleRepository
{
    private readonly BlogContext _context;

    public ArticleRepository(BlogContext context)
    {
        _context = context;
    }

    public IEnumerable<Article> GetArticles()
    {
        return _context.Articles.Include(a => a.Category).ToList();
    }

    public void Add(Article article)
    {
        _context.Articles.Add(article);
        _context.SaveChanges();
    }

    public Article? GetArticleById(int id)
    {
        return _context.Articles
            .Include(a => a.Category)
            .FirstOrDefault(a => a.Id == id);
    }

    public void Update(Article article)
    {
        _context.Articles.Update(article);
        _context.SaveChanges();
    }
}
