using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blogify.ClientApi.Models;
using Blogify.ClientApi.ViewModel;

namespace Blogify.ClientApi.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // 建立假資料用於展示，之後會替換為真實資料庫查詢
        var homeViewModel = new HomeViewModel
        {
            SiteTitle = "MyBlog",
            SiteDescription = "專業的技術部落格平台，分享最新的程式設計知識和開發經驗",
            WelcomeMessage = "歡迎來到 MyBlog",
            TotalArticles = 25,
            TotalCategories = 8,
            LatestArticlesCount = 6
        };

        // 建立假分類資料
        homeViewModel.Categories = new List<CategoryViewModel>
        {
            new CategoryViewModel { Id = 1, Name = "C# 開發", Description = "C# 程式設計相關文章", ArticleCount = 8, Slug = "csharp-development" },
            new CategoryViewModel { Id = 2, Name = "前端技術", Description = "JavaScript、HTML、CSS 相關技術", ArticleCount = 6, Slug = "frontend-tech" },
            new CategoryViewModel { Id = 3, Name = "資料庫", Description = "SQL Server、MySQL 等資料庫技術", ArticleCount = 4, Slug = "database" },
            new CategoryViewModel { Id = 4, Name = "雲端服務", Description = "Azure、AWS 等雲端平台", ArticleCount = 3, Slug = "cloud-services" },
            new CategoryViewModel { Id = 5, Name = "DevOps", Description = "CI/CD、容器化等開發維運", ArticleCount = 2, Slug = "devops" },
            new CategoryViewModel { Id = 6, Name = "工具介紹", Description = "開發工具與實用軟體介紹", ArticleCount = 2, Slug = "tools" }
        };

        // 建立假文章資料
        homeViewModel.LatestArticles = new List<HomeArticleViewModel>
        {
            new HomeArticleViewModel
            {
                Id = 1,
                Title = "ASP.NET Core 8 新功能完整指南",
                Summary = "深入探討 ASP.NET Core 8 的最新功能和改進",
                Preview = "ASP.NET Core 8 帶來了許多令人興奮的新功能，包括更好的效能、新的 API 端點和改進的開發者體驗。本文將帶您深入了解這些新特性...",
                PublishedDate = DateTime.Now.AddDays(-2),
                CategoryName = "C# 開發",
                AuthorName = "技術團隊",
                ViewCount = 1205,
                Slug = "aspnet-core-8-new-features-guide",
                Tags = new List<string> { "ASP.NET Core", "C#", "Web 開發", ".NET 8" },
                IsFeatured = true
            },
            new HomeArticleViewModel
            {
                Id = 2,
                Title = "JavaScript ES2024 新特性解析",
                Summary = "了解 JavaScript 最新版本的新語法和功能",
                Preview = "JavaScript ES2024 版本引入了許多實用的新特性，包括新的陣列方法、改進的錯誤處理和更好的異步程式設計支援。讓我們一起探索這些新功能...",
                PublishedDate = DateTime.Now.AddDays(-5),
                CategoryName = "前端技術",
                AuthorName = "前端專家",
                ViewCount = 892,
                Slug = "javascript-es2024-new-features",
                Tags = new List<string> { "JavaScript", "ES2024", "前端", "Web 開發" },
                IsFeatured = false
            },
            new HomeArticleViewModel
            {
                Id = 3,
                Title = "Entity Framework Core 效能最佳化技巧",
                Summary = "提升 EF Core 查詢效能的實用技巧和最佳實踐",
                Preview = "Entity Framework Core 是 .NET 生態系統中最受歡迎的 ORM 工具之一。本文將分享如何最佳化 EF Core 的查詢效能...",
                PublishedDate = DateTime.Now.AddDays(-7),
                CategoryName = "資料庫",
                AuthorName = "資料庫專家",
                ViewCount = 756,
                Slug = "ef-core-performance-optimization",
                Tags = new List<string> { "Entity Framework", "效能最佳化", "資料庫", ".NET" },
                IsFeatured = true
            },
            new HomeArticleViewModel
            {
                Id = 4,
                Title = "Docker 容器化應用程式部署實戰",
                Summary = "從零開始學習 Docker 容器化技術",
                Preview = "Docker 已成為現代應用程式部署的標準工具。本文將透過實際範例，帶您了解如何使用 Docker 容器化您的應用程式...",
                PublishedDate = DateTime.Now.AddDays(-10),
                CategoryName = "DevOps",
                AuthorName = "DevOps 工程師",
                ViewCount = 634,
                Slug = "docker-containerization-deployment",
                Tags = new List<string> { "Docker", "容器化", "部署", "DevOps" },
                IsFeatured = false
            },
            new HomeArticleViewModel
            {
                Id = 5,
                Title = "Azure Functions 無伺服器架構入門",
                Summary = "學習使用 Azure Functions 建立無伺服器應用程式",
                Preview = "無伺服器架構是雲端運算的重要趨勢。Azure Functions 提供了一個強大的平台來建立事件驅動的應用程式...",
                PublishedDate = DateTime.Now.AddDays(-12),
                CategoryName = "雲端服務",
                AuthorName = "雲端架構師",
                ViewCount = 521,
                Slug = "azure-functions-serverless-intro",
                Tags = new List<string> { "Azure Functions", "無伺服器", "雲端", "架構" },
                IsFeatured = false
            },
            new HomeArticleViewModel
            {
                Id = 6,
                Title = "Visual Studio Code 擴充功能推薦",
                Summary = "提升開發效率的 VS Code 擴充功能清單",
                Preview = "Visual Studio Code 是目前最受歡迎的程式編輯器之一。透過安裝適當的擴充功能，可以大幅提升開發效率...",
                PublishedDate = DateTime.Now.AddDays(-15),
                CategoryName = "工具介紹",
                AuthorName = "開發者",
                ViewCount = 445,
                Slug = "vscode-extensions-recommendations",
                Tags = new List<string> { "VS Code", "擴充功能", "開發工具", "效率" },
                IsFeatured = false
            }
        };

        // 精選文章 (取前 3 篇標記為精選的文章)
        homeViewModel.FeaturedArticles = homeViewModel.LatestArticles
            .Where(a => a.IsFeatured)
            .Take(3)
            .ToList();

        // 熱門文章 (按閱讀次數排序)
        homeViewModel.PopularArticles = homeViewModel.LatestArticles
            .OrderByDescending(a => a.ViewCount)
            .Take(5)
            .ToList();

        return View(homeViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Route("article/{slug}")]
    public IActionResult Article(string slug)
    {
        // 假資料查詢，實際應用請改為資料庫查詢
        var article = GetAllArticles().FirstOrDefault(a => a.Slug == slug);
        if (article == null)
        {
            return NotFound();
        }
        
        // 增加閱讀次數 (模擬)
        article.ViewCount++;
        
        return View(article);
    }

    /// <summary>
    /// 根據文章 ID 取得文章詳情
    /// </summary>
    /// <param name="id">文章 ID</param>
    /// <returns>文章詳情頁面</returns>
    [Route("article/{id:int}")]
    public IActionResult ArticleById(int id)
    {
        var article = GetAllArticles().FirstOrDefault(a => a.Id == id);
        if (article == null)
        {
            return NotFound();
        }
        
        // 增加閱讀次數 (模擬)
        article.ViewCount++;
        
        return View("Article", article);
    }

    /// <summary>
    /// 文章列表頁面
    /// </summary>
    /// <returns>文章列表頁面</returns>
    public IActionResult Articles()
    {
        var articles = GetAllArticles();
        var articlesViewModel = new ArticlesViewModel
        {
            Articles = articles,
            TotalArticles = articles.Count,
            CurrentPage = 1,
            PageSize = 10,
            TotalPages = (int)Math.Ceiling(articles.Count / 10.0)
        };

        return View(articlesViewModel);
    }

    /// <summary>
    /// 根據分類查看文章
    /// </summary>
    /// <param name="slug">分類 slug</param>
    /// <returns>分類文章列表</returns>
    [Route("category/{slug}")]
    public IActionResult Category(string slug)
    {
        var articles = GetAllArticles();
        var categoryArticles = articles.Where(a => a.CategoryName.ToLower().Replace(" ", "-") == slug).ToList();
        
        if (!categoryArticles.Any())
        {
            return NotFound();
        }

        var categoryName = categoryArticles.First().CategoryName;
        var articlesViewModel = new ArticlesViewModel
        {
            Articles = categoryArticles,
            TotalArticles = categoryArticles.Count,
            CurrentPage = 1,
            PageSize = 10,
            TotalPages = (int)Math.Ceiling(categoryArticles.Count / 10.0),
            CategoryName = categoryName,
            CategorySlug = slug
        };

        return View("Articles", articlesViewModel);
    }

    // 取得所有文章的假資料方法
    private List<HomeArticleViewModel> GetAllArticles()
    {
        return new List<HomeArticleViewModel>
        {
            new HomeArticleViewModel
            {
                Id = 1,
                Title = "ASP.NET Core 8 新功能完整指南",
                Summary = "深入探討 ASP.NET Core 8 的最新功能和改進",
                Preview = "ASP.NET Core 8 帶來了許多令人興奮的新功能，包括更好的效能、新的 API 端點和改進的開發者體驗。本文將帶您深入了解這些新特性...",
                PublishedDate = DateTime.Now.AddDays(-2),
                CategoryName = "C# 開發",
                AuthorName = "技術團隊",
                ViewCount = 1205,
                Slug = "aspnet-core-8-new-features-guide",
                Tags = new List<string> { "ASP.NET Core", "C#", "Web 開發", ".NET 8" },
                IsFeatured = true
            },
            new HomeArticleViewModel
            {
                Id = 2,
                Title = "JavaScript ES2024 新特性解析",
                Summary = "了解 JavaScript 最新版本的新語法和功能",
                Preview = "JavaScript ES2024 版本引入了許多實用的新特性，包括新的陣列方法、改進的錯誤處理和更好的異步程式設計支援。讓我們一起探索這些新功能...",
                PublishedDate = DateTime.Now.AddDays(-5),
                CategoryName = "前端技術",
                AuthorName = "前端專家",
                ViewCount = 892,
                Slug = "javascript-es2024-new-features",
                Tags = new List<string> { "JavaScript", "ES2024", "前端", "Web 開發" },
                IsFeatured = false
            },
            new HomeArticleViewModel
            {
                Id = 3,
                Title = "Entity Framework Core 效能最佳化技巧",
                Summary = "提升 EF Core 查詢效能的實用技巧和最佳實踐",
                Preview = "Entity Framework Core 是 .NET 生態系統中最受歡迎的 ORM 工具之一。本文將分享如何最佳化 EF Core 的查詢效能...",
                PublishedDate = DateTime.Now.AddDays(-7),
                CategoryName = "資料庫",
                AuthorName = "資料庫專家",
                ViewCount = 756,
                Slug = "ef-core-performance-optimization",
                Tags = new List<string> { "Entity Framework", "效能最佳化", "資料庫", ".NET" },
                IsFeatured = true
            },
            new HomeArticleViewModel
            {
                Id = 4,
                Title = "Docker 容器化應用程式部署實戰",
                Summary = "從零開始學習 Docker 容器化技術",
                Preview = "Docker 已成為現代應用程式部署的標準工具。本文將透過實際範例，帶您了解如何使用 Docker 容器化您的應用程式...",
                PublishedDate = DateTime.Now.AddDays(-10),
                CategoryName = "DevOps",
                AuthorName = "DevOps 工程師",
                ViewCount = 634,
                Slug = "docker-containerization-deployment",
                Tags = new List<string> { "Docker", "容器化", "部署", "DevOps" },
                IsFeatured = false
            },
            new HomeArticleViewModel
            {
                Id = 5,
                Title = "Azure Functions 無伺服器架構入門",
                Summary = "學習使用 Azure Functions 建立無伺服器應用程式",
                Preview = "無伺服器架構是雲端運算的重要趨勢。Azure Functions 提供了一個強大的平台來建立事件驅動的應用程式...",
                PublishedDate = DateTime.Now.AddDays(-12),
                CategoryName = "雲端服務",
                AuthorName = "雲端架構師",
                ViewCount = 521,
                Slug = "azure-functions-serverless-intro",
                Tags = new List<string> { "Azure Functions", "無伺服器", "雲端", "架構" },
                IsFeatured = false
            },
            new HomeArticleViewModel
            {
                Id = 6,
                Title = "Visual Studio Code 擴充功能推薦",
                Summary = "提升開發效率的 VS Code 擴充功能清單",
                Preview = "Visual Studio Code 是目前最受歡迎的程式編輯器之一。透過安裝適當的擴充功能，可以大幅提升開發效率...",
                PublishedDate = DateTime.Now.AddDays(-15),
                CategoryName = "工具介紹",
                AuthorName = "開發者",
                ViewCount = 445,
                Slug = "vscode-extensions-recommendations",
                Tags = new List<string> { "VS Code", "擴充功能", "開發工具", "效率" },
                IsFeatured = false
            }
        };
    }
}
