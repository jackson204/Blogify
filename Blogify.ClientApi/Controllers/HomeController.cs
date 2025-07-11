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

    public IActionResult Index(int page = 1)
    {
        // 取得所有文章資料
        var allArticles = GetAllArticles();
        // 依照日期排序，最新在前
        var sortedArticles = allArticles.OrderByDescending(a => a.PublishedDate).ToList();
        
        // 分頁設定
        const int pageSize = 5;
        var totalArticles = sortedArticles.Count;
        var totalPages = (int)Math.Ceiling(totalArticles / (double)pageSize);
        page = Math.Max(1, Math.Min(page, totalPages)); // 確保 page 在合法範圍
        
        // 取得目前頁面的文章
        var pagedArticles = sortedArticles
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        // 建立假分類資料
        var categories = GetAllCategories();
        
        // 計算每個分類的文章數量
        foreach (var category in categories)
        {
            category.ArticleCount = sortedArticles.Count(a => a.CategoryName == category.Name);
        }
        
        // 建立假資料用於展示，之後會替換為真實資料庫查詢
        var homeViewModel = new HomeViewModel
        {
            SiteTitle = "MyBlog",
            SiteDescription = "專業的技術部落格平台，分享最新的程式設計知識和開發經驗",
            WelcomeMessage = "歡迎來到 MyBlog",
            TotalArticles = sortedArticles.Count, // 使用實際文章數量
            TotalCategories = categories.Count, // 從假資料計算分類數量
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            Categories = categories // 設定分類資料
        };

        // 使用分頁後的文章資料
        homeViewModel.LatestArticles = pagedArticles;

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

    /// <summary>
    /// 關於頁面
    /// </summary>
    /// <returns>關於頁面</returns>
    public IActionResult About()
    {
        // 取得實際資料
        var allArticles = GetAllArticles();
        var categories = GetAllCategories();
        var totalViewCount = allArticles.Sum(a => a.ViewCount);
        
        // 建立動態統計資料
        var aboutViewModel = new AboutViewModel
        {
            SiteTitle = "MyBlog",
            SiteDescription = "專業的技術部落格平台，分享最新的程式設計知識和開發經驗",
            ContactEmail = "contact@myblog.com",
            EstablishedDate = new DateTime(2024, 1, 1),
            Statistics = new Dictionary<string, int>
            {
                { "總文章數", allArticles.Count },
                { "文章分類", categories.Count },
                { "總閱讀量", totalViewCount },
                { "註冊用戶", 326 } // 這個暫時保持靜態，因為目前沒有用戶系統
            }
        };

        return View(aboutViewModel);
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
    /// <param name="page">目前頁數（預設 1）</param>
    /// <returns>文章列表頁面</returns>
    public IActionResult Articles(int page = 1)
    {
        const int pageSize = 5;
        var articles = GetAllArticles();
        var totalArticles = articles.Count;
        var totalPages = (int)Math.Ceiling(totalArticles / (double)pageSize);
        page = Math.Max(1, Math.Min(page, totalPages)); // 確保 page 在合法範圍
        var pagedArticles = articles
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var articlesViewModel = new ArticlesViewModel
        {
            Articles = pagedArticles,
            TotalArticles = totalArticles,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = totalPages
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
        // 取得所有文章
        var articles = GetAllArticles();
        // 取得所有分類 - 使用統一的分類資料
        var categories = GetAllCategories();

        // 找到對應分類
        var category = categories.FirstOrDefault(c => c.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
        if (category is null)
        {
            // 找不到分類，回傳 404
            return NotFound($"找不到分類：{slug}");
        }

        // 篩選該分類下的文章 - 用分類名稱比較
        var categoryArticles = articles.Where(a => a.CategoryName == category.Name).ToList();

        var articlesViewModel = new ArticlesViewModel
        {
            Articles = categoryArticles,
            TotalArticles = categoryArticles.Count,
            CurrentPage = 1,
            PageSize = 10,
            TotalPages = (int)Math.Ceiling(categoryArticles.Count / 10.0),
            CategoryName = category.Name,
            CategorySlug = category.Slug
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
                PublishedDate = new DateTime(2025, 7, 1),
                CategoryName = "後端開發", // 統一使用與 AdminApi 相同的分類名稱
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
                PublishedDate = new DateTime(2024, 12, 15),
                CategoryName = "前端開發", // 統一分類名稱
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
                PublishedDate = new DateTime(2023, 11, 20),
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
                PublishedDate = new DateTime(2025, 6, 10),
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
                PublishedDate = new DateTime(2024, 8, 5),
                CategoryName = "DevOps", // 改為 DevOps 分類
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
                PublishedDate = new DateTime(2023, 5, 18),
                CategoryName = "前端開發", // 改為前端開發
                AuthorName = "開發者",
                ViewCount = 445,
                Slug = "vscode-extensions-recommendations",
                Tags = new List<string> { "VS Code", "擴充功能", "開發工具", "效率" },
                IsFeatured = false
            },
            new HomeArticleViewModel
            {
                Id = 7,
                Title = "Blazor WebAssembly 實戰教學",
                Summary = "從入門到進階，打造互動式 Web 應用",
                Preview = "Blazor WebAssembly 讓 C# 開發者能夠直接在瀏覽器中撰寫前端程式。本文將帶你一步步打造 Blazor 應用...",
                PublishedDate = new DateTime(2025, 2, 28),
                CategoryName = "前端開發", // 改為前端開發
                AuthorName = "全端工程師",
                ViewCount = 980,
                Slug = "blazor-webassembly-tutorial",
                Tags = new List<string> { "Blazor", "WebAssembly", "C#", "前端" },
                IsFeatured = true
            },
            new HomeArticleViewModel
            {
                Id = 8,
                Title = "React 18 新功能與最佳實踐",
                Summary = "掌握 React 18 的新特性與效能優化技巧",
                Preview = "React 18 帶來了自動批次更新、Concurrent Rendering 等新功能，本文將介紹如何善用這些特性...",
                PublishedDate = new DateTime(2024, 3, 10),
                CategoryName = "前端開發",
                AuthorName = "前端專家",
                ViewCount = 1100,
                Slug = "react-18-new-features-best-practices",
                Tags = new List<string> { "React", "前端", "效能優化" },
                IsFeatured = false
            },
            new HomeArticleViewModel
            {
                Id = 9,
                Title = "SQL Server 2022 新功能解析",
                Summary = "探索 SQL Server 2022 的創新功能",
                Preview = "SQL Server 2022 推出多項新功能，包括更強的安全性與雲端整合，本文將帶你快速掌握...",
                PublishedDate = new DateTime(2023, 9, 30),
                CategoryName = "資料庫",
                AuthorName = "資料庫專家",
                ViewCount = 670,
                Slug = "sql-server-2022-new-features",
                Tags = new List<string> { "SQL Server", "資料庫", "新功能" },
                IsFeatured = false
            },
            new HomeArticleViewModel
            {
                Id = 10,
                Title = "Kubernetes 叢集部署全攻略",
                Summary = "從基礎到進階，掌握 K8s 部署技巧",
                Preview = "Kubernetes 已成為雲端原生應用的核心，本文將介紹如何部署與管理 K8s 叢集...",
                PublishedDate = new DateTime(2025, 5, 20),
                CategoryName = "DevOps",
                AuthorName = "雲端架構師",
                ViewCount = 800,
                Slug = "kubernetes-cluster-deployment-guide",
                Tags = new List<string> { "Kubernetes", "DevOps", "雲端" },
                IsFeatured = true
            },
            new HomeArticleViewModel
            {
                Id = 11,
                Title = "AWS Lambda 實戰應用",
                Summary = "深入了解 AWS Lambda 的應用場景與最佳實踐",
                Preview = "AWS Lambda 是無伺服器運算的代表，本文將介紹其架構、觸發方式與實際案例...",
                PublishedDate = new DateTime(2024, 10, 8),
                CategoryName = "DevOps", // 改為 DevOps
                AuthorName = "雲端專家",
                ViewCount = 590,
                Slug = "aws-lambda-in-action",
                Tags = new List<string> { "AWS", "Lambda", "雲端", "Serverless" },
                IsFeatured = false
            },
            new HomeArticleViewModel
            {
                Id = 12,
                Title = "Git 進階操作與團隊協作技巧",
                Summary = "提升團隊開發效率的 Git 技巧",
                Preview = "Git 是現代軟體開發不可或缺的工具，本文將介紹進階操作與團隊協作的最佳實踐...",
                PublishedDate = new DateTime(2023, 2, 14),
                CategoryName = "DevOps", // 改為 DevOps
                AuthorName = "開發者",
                ViewCount = 410,
                Slug = "git-advanced-team-collaboration",
                Tags = new List<string> { "Git", "版本控制", "團隊協作" },
                IsFeatured = false
            },
            new HomeArticleViewModel
            {
                Id = 13,
                Title = "CI/CD 自動化流程建置實戰",
                Summary = "打造高效 DevOps 流程的實用指南",
                Preview = "CI/CD 是現代軟體開發流程的核心，本文將帶你從零開始建置自動化流程...",
                PublishedDate = new DateTime(2024, 6, 22),
                CategoryName = "DevOps",
                AuthorName = "DevOps 工程師",
                ViewCount = 720,
                Slug = "cicd-automation-practice",
                Tags = new List<string> { "CI/CD", "自動化", "DevOps" },
                IsFeatured = true
            }
        };
    }

    /// <summary>
    /// 取得所有分類資料 - 與 AdminApi 保持一致
    /// </summary>
    /// <returns>分類列表</returns>
    private List<CategoryViewModel> GetAllCategories()
    {
        return new List<CategoryViewModel>
        {
            new CategoryViewModel { Id = 1, Name = "前端開發", Description = "前端技術相關文章", Slug = "frontend-development" },
            new CategoryViewModel { Id = 2, Name = "後端開發", Description = "後端技術相關文章", Slug = "backend-development" },
            new CategoryViewModel { Id = 3, Name = "資料庫", Description = "資料庫技術相關文章", Slug = "database" },
            new CategoryViewModel { Id = 4, Name = "DevOps", Description = "DevOps 相關文章", Slug = "devops" },
            new CategoryViewModel { Id = 5, Name = "行動開發", Description = "行動應用程式開發相關文章", Slug = "mobile-development" }
        };
    }
}
