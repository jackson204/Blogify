using Blogify.AdminApi.DTO;
using Blogify.AdminApi.Models;
using Blogify.AdminApi.Models.Repository;
using Blogify.AdminApi.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Blogify.AdminApi.Controllers;

public class ArticleController : Controller
{
    private readonly ArticleRepository _articleRepository;

    public ArticleController(ArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public IActionResult List(string? search = null, string? status = null, int? category = null, string? sort = null, int page = 1)
    {
        var articles = _articleRepository.GetArticles();

        // 建立篩選物件
        var filter = new ArticleFilterDto
        {
            SearchKeyword = search,
            Status = status,
            CategoryId = category,
            SortBy = sort ?? "UpdatedAt",
            SortDirection = "desc", // 預設降序排列
            Page = page,
            PageSize = 10 // 設定每頁顯示數量
        };

        // 根據篩選條件過濾文章
        if (!string.IsNullOrEmpty(filter.SearchKeyword))
        {
            articles = articles.Where(a =>
                a.Title.Contains(filter.SearchKeyword, StringComparison.OrdinalIgnoreCase) ||
                a.Content.Contains(filter.SearchKeyword, StringComparison.OrdinalIgnoreCase) ||
                a.Author.Contains(filter.SearchKeyword, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(filter.Status))
        {
            articles = articles.Where(a => a.Status == filter.Status);
        }

        if (filter.CategoryId.HasValue)
        {
            articles = articles.Where(a => a.CategoryId == filter.CategoryId.Value);
        }

        // 排序
        switch(filter.SortBy.ToLower())
        {
            case "title":
                articles = articles.OrderByDescending(a => a.Title);
                break;

            case "author":
                articles = articles.OrderByDescending(a => a.Author);
                break;

            case "views":
                articles = articles.OrderByDescending(a => a.Views);
                break;

            case "updatedat":
            default:
                articles = articles.OrderByDescending(a => a.UpdatedAt);
                break;
        }

        // 分頁處理

        var totalCount = articles.Count();
        var totalPages = (int) Math.Ceiling((double) totalCount / filter.PageSize);

        var pagedArticles = articles
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize);
        var viewModel = pagedArticles
            .Select(article => new ArticleListItemViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Excerpt = article.Excerpt,
                Author = article.Author,
                CategoryName = article.Category?.Name,
                Tags = article.Tags,
                ReadTime = article.ReadTime,
                Image = article.Image,
                Status = article.Status,
                Featured = article.Featured,
                Views = article.Views,
                UpdatedAt = article.UpdatedAt
            })
            .ToList();
        ViewBag.Categories = CategoryRepository.Categories();

        return View(viewModel);
    }

    public IActionResult Create()
    {
        ViewBag.Action = nameof(Create);
        ViewBag.Categories = CategoryRepository.Categories();
        return View(new ArticleViewModel());
    }

    [HttpPost]
    public IActionResult Create(ArticleViewModel model)
    {
        var article = new Article()
        {
            Title = model.Title,
            Content = model.Content,
            Excerpt = model.Excerpt ?? string.Empty, // 避免 Excerpt 為 null
            Author = model.Author ?? string.Empty,   // 修正：避免 Author 為 null
            Tags = model.Tags ?? string.Empty,       // 避免 Tags 為 null
            ReadTime = model.ReadTime,
            Image = model.Image,
            Status = model.Status,
            Featured = model.Featured,
            CategoryId = model.Category,
            UpdatedAt = DateTime.Now
        };

        _articleRepository.Add(article);
        return RedirectToAction(nameof(List));
    }

    public IActionResult Edit(int id)
    {
        var article = _articleRepository.GetArticleById(id);
        if (article == null)
        {
            return NotFound();
        }

        ViewBag.Action = nameof(Edit);
        ViewBag.Categories = CategoryRepository.Categories();

        var viewModel = new ArticleViewModel
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Excerpt = article.Excerpt,
            Author = article.Author,
            Tags = article.Tags,
            ReadTime = article.ReadTime,
            Image = article.Image,
            Status = article.Status,
            Featured = article.Featured,
            Category = article.CategoryId
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Edit(int id, ArticleViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var article = _articleRepository.GetArticleById(id);
        if (article == null)
        {
            return NotFound();
        }

        article.Title = model.Title;
        article.Content = model.Content;
        article.Excerpt = model.Excerpt ?? string.Empty; // 避免 Excerpt 為 null
        article.Author = model.Author ?? string.Empty;   // 修正：避免 Author 為 null
        article.Tags = model.Tags ?? string.Empty;       // 避免 Tags 為 null
        article.ReadTime = model.ReadTime;
        article.Image = model.Image;
        article.Status = model.Status;
        article.Featured = model.Featured;
        article.CategoryId = model.Category;
        article.UpdatedAt = DateTime.Now;

        _articleRepository.Update(article);
        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var article = _articleRepository.GetArticleById(id);
        if (article == null)
        {
            return NotFound();
        }

        _articleRepository.Delete(article);
        return RedirectToAction(nameof(List));
    }
}
