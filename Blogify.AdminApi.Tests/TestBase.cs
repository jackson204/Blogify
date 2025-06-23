using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Blogify.AdminApi.Data;
using Blogify.AdminApi.Models;

namespace Blogify.AdminApi.Tests;

/// <summary>
/// 測試基礎類別，設定測試環境
/// </summary>
public class TestBase : IDisposable
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;
    private readonly string DatabaseName = "TestDatabase_" + Guid.NewGuid();

    public TestBase()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // 移除原本的 DbContext
                    services.RemoveAll(typeof(DbContextOptions<BlogContext>));

                    // 使用共享的 In-Memory 資料庫名稱
                    services.AddDbContext<BlogContext>(options =>
                    {
                        options.UseInMemoryDatabase(databaseName: DatabaseName);
                    });
                });
            });

        _client = _factory.CreateClient();

        // 初始化測試資料
        SeedTestData();
    }

    /// <summary>
    /// 初始化測試資料
    /// </summary>
    private void SeedTestData()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BlogContext>();

        // 確保資料庫是乾淨的
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // 建立測試分類
        var categories = new[]
        {
            new Category { Id = 1, Name = "前端開發" },
            new Category { Id = 2, Name = "後端開發" },
            new Category { Id = 3, Name = "資料庫" }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        // 建立測試文章
        var articles = new[]
        {
            new Article
            {
                Title = "JavaScript ES6+ 新特性完整指南",
                Content = "JavaScript ES6+ 帶來了許多強大的新特性，讓開發者能夠寫出更簡潔、更高效的程式碼。",
                Excerpt = "深入了解 JavaScript ES6+ 的新特性",
                Author = "技術小編",
                CategoryId = 1,
                Tags = "JavaScript,ES6,前端",
                ReadTime = 8,
                Status = "published",
                Featured = true,
                Views = 1250,
                UpdatedAt = new DateTime(2024, 1, 15, 10, 30, 0)
            },
            new Article
            {
                Title = "CSS Grid 與 Flexbox 完整比較",
                Content = "CSS Grid 和 Flexbox 都是現代 CSS 佈局的重要工具。",
                Excerpt = "詳細比較 CSS Grid 和 Flexbox 的特點",
                Author = "設計師小王",
                CategoryId = 1,
                Tags = "CSS,Grid,Flexbox",
                ReadTime = 6,
                Status = "published",
                Featured = false,
                Views = 980,
                UpdatedAt = new DateTime(2024, 1, 12, 14, 20, 0)
            },
            new Article
            {
                Title = "Node.js 效能優化實戰技巧",
                Content = "Node.js 應用的效能優化是後端開發的重要課題。",
                Excerpt = "分享 Node.js 效能優化的實戰經驗",
                Author = "後端工程師",
                CategoryId = 2,
                Tags = "Node.js,效能優化,後端",
                ReadTime = 10,
                Status = "published",
                Featured = true,
                Views = 1580,
                UpdatedAt = new DateTime(2024, 1, 10, 9, 15, 0)
            },
            new Article
            {
                Title = "React Hooks 深度解析",
                Content = "React Hooks 徹底改變了 React 開發的方式。",
                Excerpt = "深入探討 React Hooks 的使用方法",
                Author = "React 專家",
                CategoryId = 1,
                Tags = "React,Hooks,前端",
                ReadTime = 12,
                Status = "draft",
                Featured = false,
                Views = 2100,
                UpdatedAt = new DateTime(2024, 1, 8, 16, 45, 0)
            },
            new Article
            {
                Title = "MongoDB 資料庫設計最佳實踐",
                Content = "MongoDB 作為 NoSQL 資料庫，有其獨特的設計原則。",
                Excerpt = "分享 MongoDB 資料庫設計的最佳實踐",
                Author = "資料庫專家",
                CategoryId = 3,
                Tags = "MongoDB,NoSQL,資料庫設計",
                ReadTime = 9,
                Status = "published",
                Featured = false,
                Views = 890,
                UpdatedAt = new DateTime(2024, 1, 5, 11, 30, 0)
            }
        };

        context.Articles.AddRange(articles);
        context.SaveChanges();

        // 驗證資料已正確儲存
        var savedArticleCount = context.Articles.Count();
        var savedCategoryCount = context.Categories.Count();
        Console.WriteLine($"Test data seeded: {savedArticleCount} articles, {savedCategoryCount} categories");
    }

    public void Dispose()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}
