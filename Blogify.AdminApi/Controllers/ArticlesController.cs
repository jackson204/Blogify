using Blogify.AdminApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blogify.AdminApi.Controllers;

[Route("[controller]")]
public class ArticlesController : Controller
{
    private readonly BlogContext _context;

    public ArticlesController(BlogContext context)
    {
        _context = context;
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] ArticleCreateDtoRequest request)
    {
        var article = new Article
        {
            Title = request.Title,
            Summary = request.Summary,
            Author = request.Author,
            CategoryId = request.CategoryId,
            ReadingTimeMinutes = request.ReadingTimeMinutes,
            ImageUrl = request.ImageUrl,
            Status = (ArticleStatus) request.Status,
            IsFeatured = request.IsFeatured,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // 處理標籤
        if (request.TagNames is { Count: > 0 })
        {
            var tags = _context.Tags.Where(t => request.TagNames.Contains(t.Name)).ToList();
            var existingTagNames = tags.Select(t => t.Name).ToList();
            var newTagNames = request.TagNames.Except(existingTagNames).ToList();
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
        return RedirectToAction("Index", "Home");
    }
}
