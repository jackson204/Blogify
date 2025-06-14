using Blogify.AdminApi.Models;
using Blogify.AdminApi.ViewModels;
using Blogify.AdminApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blogify.AdminApi.Controllers;

[Route("admin/articles")]
public class ArticlesController : Controller
{
    private readonly BlogContext _context;
    private readonly IArticleService _articleService;

    public ArticlesController(BlogContext context, IArticleService articleService)
    {
        _context = context;
        _articleService = articleService;
    }

    [HttpGet("List")]
    public async Task<IActionResult> List()
    {
        var articles = await _articleService.GetArticleListAsync();
        return View(articles);
    }
    
    [HttpGet("Create")]
    public IActionResult Create()
    {
        // 假資料分類清單
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "前端開發" },
            new Category { Id = 2, Name = "後端開發" },
            new Category { Id = 3, Name = "資料庫" },
            new Category { Id = 4, Name = "DevOps" },
            new Category { Id = 5, Name = "行動開發" }
        };
        ViewBag.Categories = categories;
        return View();
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] ArticleCreateViewModel model)
    {
        var article = new Article
        {
            Title = model.Title,
            Summary = model.Summary,
            Author = model.Author,
            CategoryId = model.CategoryId ?? 0,
            ReadingTimeMinutes = model.ReadingTimeMinutes ?? 0,
            ImageUrl = model.ImageUrl,
            Status = model.Status == "published" ? ArticleStatus.Published : ArticleStatus.Draft,
            IsFeatured = model.IsFeatured,
            Content = model.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // 處理標籤
        if (!string.IsNullOrWhiteSpace(model.TagNames))
        {
            var tagNames = model.TagNames.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            var tags = _context.Tags.Where(t => tagNames.Contains(t.Name)).ToList();
            var existingTagNames = tags.Select(t => t.Name).ToList();
            var newTagNames = tagNames.Except(existingTagNames).ToList();
            foreach (var tagName in newTagNames)
            {
                var newTag = new Tag { Name = tagName };
                _context.Tags.Add(newTag);
                tags.Add(newTag);
            }
            article.Tags = tags;
        }

        _context.Articles.Add(article);
        await _context.SaveChangesAsync();
        return RedirectToAction("List");
    }
}
