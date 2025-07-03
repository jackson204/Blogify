using Blogify.AdminApi.DTO;
using Blogify.AdminApi.Models;
using Blogify.AdminApi.Models.Repository;
using Blogify.AdminApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Blogify.AdminApi.Controllers;

[Authorize]
public class ArticleController : Controller
{
    private readonly ArticleRepository _articleRepository;
    private readonly CategoryRepository _categoryRepository;

    public ArticleController(ArticleRepository articleRepository, CategoryRepository categoryRepository)
    {
        _articleRepository = articleRepository;
        _categoryRepository = categoryRepository;
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
            
        var articleViewModels = pagedArticles
            .Select(article => new ArticleListItemViewModel
            {
                Id = article.Id,
                Title = article.Title ?? string.Empty,
                Excerpt = article.Excerpt ?? string.Empty,
                Author = article.Author ?? string.Empty,
                CategoryName = article.Category?.Name,
                Tags = article.Tags ?? string.Empty,
                ReadTime = article.ReadTime,
                Image = article.Image ?? string.Empty,
                Status = article.Status ?? string.Empty,
                Featured = article.Featured,
                Views = article.Views,
                UpdatedAt = article.UpdatedAt
            })
            .ToList();
            
        // 建立完整的視圖模型
        var viewModel = new ArticleListViewModel
        {
            Articles = articleViewModels,
            Filter = filter,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
        
        // 將查詢條件傳遞給 View 以保持表單狀態
        ViewBag.Categories = _categoryRepository.GetCategories();
        ViewBag.CurrentSearch = filter.SearchKeyword;
        ViewBag.CurrentStatus = filter.Status;
        ViewBag.CurrentCategory = filter.CategoryId;
        ViewBag.CurrentSort = filter.SortBy;
        ViewBag.CurrentPage = filter.Page;

        return View(viewModel);
    }

    public IActionResult Create(string? search = null, string? status = null, int? category = null, string? sort = null, int page = 1)
    {
        ViewBag.Action = Url.Action("Create", "Article");
        ViewBag.Categories = _categoryRepository.GetCategories();
        
        // 建立取消按鈕的返回 URL，包含查詢參數
        ViewBag.CancelUrl = Url.Action("List", "Article", new { 
            search = search, 
            status = status, 
            category = category, 
            sort = sort, 
            page = page 
        });
        
        return View(new ArticleViewModel());
    }

    [HttpPost]
    public IActionResult Create(ArticleViewModel model, string? search = null, string? status = null, int? category = null, string? sort = null, int page = 1)
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
        
        // 保持查詢條件
        return RedirectToAction(nameof(List), new { 
            search = search, 
            status = status, 
            category = category, 
            sort = sort, 
            page = page 
        });
    }

    public IActionResult Edit(int id, string? search = null, string? status = null, int? category = null, string? sort = null, int page = 1)
    {
        var article = _articleRepository.GetArticleById(id);
        if (article == null)
        {
            return NotFound();
        }
        ViewBag.Action = Url.Action("Edit", "Article", new { id = id });
        ViewBag.Categories = _categoryRepository.GetCategories();
        
        // 建立取消按鈕的返回 URL，包含查詢參數
        ViewBag.CancelUrl = Url.Action("List", "Article", new { 
            search = search, 
            status = status, 
            category = category, 
            sort = sort, 
            page = page 
        });

        var viewModel = new ArticleViewModel
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Excerpt = article.Excerpt ?? string.Empty,
            Author = article.Author,
            Tags = article.Tags ?? string.Empty,
            ReadTime = article.ReadTime,
            Image = article.Image ?? string.Empty,
            Status = article.Status,
            Featured = article.Featured,
            Category = article.CategoryId
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Edit(int id, ArticleViewModel model, string? search = null, string? status = null, int? category = null, string? sort = null, int page = 1)
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
        article.Image = model.Image ?? string.Empty;
        article.Status = model.Status;
        article.Featured = model.Featured;
        article.CategoryId = model.Category;
        article.UpdatedAt = DateTime.Now;

        _articleRepository.Update(article);
        
        // 保持查詢條件
        return RedirectToAction(nameof(List), new { 
            search = search, 
            status = status, 
            category = category, 
            sort = sort, 
            page = page 
        });
    }

    [HttpPost]
    public IActionResult Delete(int id, string? search = null, string? status = null, int? category = null, string? sort = null, int page = 1)
    {
        var article = _articleRepository.GetArticleById(id);
        if (article == null)
        {
            return NotFound();
        }

        _articleRepository.Delete(article);
        
        // 保持查詢條件
        return RedirectToAction(nameof(List), new { 
            search = search, 
            status = status, 
            category = category, 
            sort = sort, 
            page = page 
        });
    }
}
