using Blogify.AdminApi.Models;
using Blogify.AdminApi.Models.Repository;
using Blogify.AdminApi.ViewModel;
using Blogify.AdminApi.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Blogify.AdminApi.Controllers;

public class ArticleController : Controller
{
    private readonly ArticleRepository _articleRepository;

    public ArticleController(ArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public IActionResult List(ArticleFilterDto? filter = null)
    {
        var articles = _articleRepository.GetArticles();

        // 根據篩選條件過濾文章
        if (filter != null)
        {
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
                    articles = filter.SortDirection.ToLower() == "asc"
                        ? articles.OrderBy(a => a.Title)
                        : articles.OrderByDescending(a => a.Title);
                    break;

                case "author":
                    articles = filter.SortDirection.ToLower() == "asc"
                        ? articles.OrderBy(a => a.Author)
                        : articles.OrderByDescending(a => a.Author);
                    break;

                case "views":
                    articles = filter.SortDirection.ToLower() == "asc"
                        ? articles.OrderBy(a => a.Views)
                        : articles.OrderByDescending(a => a.Views);
                    break;

                case "updatedat":
                default:
                    articles = filter.SortDirection.ToLower() == "asc"
                        ? articles.OrderBy(a => a.UpdatedAt)
                        : articles.OrderByDescending(a => a.UpdatedAt);
                    break;
            }
        }
        else
        {
            // 預設排序
            articles = articles.OrderByDescending(a => a.UpdatedAt);
        }

        var viewModel = articles
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
        ViewBag.CurrentFilter = filter ?? new ArticleFilterDto();
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

    /// <summary>
    /// 取得狀態顯示名稱
    /// </summary>
    private string GetStatusDisplayName(string status)
    {
        return status switch
        {
            "draft" => "草稿",
            "published" => "已發布",
            "archived" => "已封存",
            _ => status
        };
    }

 
    /// <summary>
    /// API 端點：取得文章列表（供前端 AJAX 呼叫）
    /// </summary>
    public IActionResult GetArticlesApi(ArticleFilterDto? filter = null)
    {
        try
        {
            // 如果是 POST 請求，從 Body 讀取參數
            if (Request.Method == "POST" && Request.HasFormContentType)
            {
                filter = new ArticleFilterDto
                {
                    SearchKeyword = Request.Form["SearchKeyword"].FirstOrDefault(),
                    Status = Request.Form["Status"].FirstOrDefault(),
                    CategoryId = int.TryParse(Request.Form["CategoryId"].FirstOrDefault(), out var catId) ? catId : null,
                    SortBy = Request.Form["SortBy"].FirstOrDefault() ?? "UpdatedAt",
                    SortDirection = Request.Form["SortDirection"].FirstOrDefault() ?? "desc",
                    Page = int.TryParse(Request.Form["Page"].FirstOrDefault(), out var page) ? page : 1,
                    PageSize = int.TryParse(Request.Form["PageSize"].FirstOrDefault(), out var size) ? size : 10
                };
            }

            var articles = _articleRepository.GetArticles();

            // 套用篩選條件
            if (filter != null)
            {
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

                // 套用排序
                switch(filter.SortBy.ToLower())
                {
                    case "title":
                        articles = filter.SortDirection.ToLower() == "asc"
                            ? articles.OrderBy(a => a.Title)
                            : articles.OrderByDescending(a => a.Title);
                        break;

                    case "author":
                        articles = filter.SortDirection.ToLower() == "asc"
                            ? articles.OrderBy(a => a.Author)
                            : articles.OrderByDescending(a => a.Author);
                        break;

                    case "views":
                        articles = filter.SortDirection.ToLower() == "asc"
                            ? articles.OrderBy(a => a.Views)
                            : articles.OrderByDescending(a => a.Views);
                        break;

                    case "updatedat":
                    default:
                        articles = filter.SortDirection.ToLower() == "asc"
                            ? articles.OrderBy(a => a.UpdatedAt)
                            : articles.OrderByDescending(a => a.UpdatedAt);
                        break;
                }
            }
            else
            {
                // 預設排序
                articles = articles.OrderByDescending(a => a.UpdatedAt);
                filter = new ArticleFilterDto(); // 建立預設篩選條件
            }

            // 計算總數
            var totalCount = articles.Count();

            // 套用分頁
            var pagedArticles = articles
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(article => new
                {
                    id = article.Id,
                    title = article.Title,
                    excerpt = article.Excerpt,
                    author = article.Author,
                    categoryName = article.Category?.Name ?? "未分類",
                    tags = article.Tags,
                    readTime = article.ReadTime,
                    image = article.Image,
                    status = article.Status,
                    featured = article.Featured,
                    views = article.Views,
                    updatedAt = article.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToList();

            return Json(new
            {
                success = true,
                data = pagedArticles,
                totalCount = totalCount,
                totalPages = (int) Math.Ceiling((double) totalCount / filter.PageSize),
                currentPage = filter.Page,
                pageSize = filter.PageSize
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"取得文章列表失敗：{ex.Message}" });
        }
    }
}
