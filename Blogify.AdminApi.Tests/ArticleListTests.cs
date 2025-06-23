using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Blogify.AdminApi.Controllers;
using Blogify.AdminApi.DTO;
using Blogify.AdminApi.Models.Repository;
using Blogify.AdminApi.ViewModel;

namespace Blogify.AdminApi.Tests;

/// <summary>
/// ArticleController List 方法的測試
/// </summary>
public class ArticleListTests : TestBase
{
    /// <summary>
    /// 取得測試用的 Controller 和 Repository
    /// </summary>
    private (ArticleController controller, ArticleRepository repository) GetTestServices()
    {
        var scope = _factory.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ArticleRepository>();
        var controller = new ArticleController(repository);
        return (controller, repository);
    }    /// <summary>
    /// 案例一：測試預設值（無篩選條件）
    /// </summary>
    [Fact]
    public void List_WithDefaultValues_ShouldReturnAllArticlesOrderedByUpdateDateDesc()
    {
        // Arrange - 不傳入任何篩選條件
        var (controller, repository) = GetTestServices();
        ArticleFilterDto? filter = null;

        // Act - 呼叫 List 方法
        var result = controller.List(filter);

        // Assert - 驗證結果
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<ArticleListItemViewModel>>(viewResult.Model);

        // 驗證回傳的文章數量（應該包含所有文章）
        Assert.Equal(5, model.Count);

        // 驗證排序（預設應該按 UpdatedAt 降序排列）
        var expectedOrder = new[]
        {
            "JavaScript ES6+ 新特性完整指南",  // 2024-01-15
            "CSS Grid 與 Flexbox 完整比較",    // 2024-01-12
            "Node.js 效能優化實戰技巧",        // 2024-01-10
            "React Hooks 深度解析",           // 2024-01-08
            "MongoDB 資料庫設計最佳實踐"       // 2024-01-05
        };

        for (int i = 0; i < expectedOrder.Length; i++)
        {
            Assert.Equal(expectedOrder[i], model[i].Title);
        }

        // 驗證 ViewBag 包含分類資料
        Assert.NotNull(viewResult.ViewData["Categories"]);
        
        // 驗證 ViewBag.CurrentFilter 是預設的 ArticleFilterDto
        var currentFilter = Assert.IsType<ArticleFilterDto>(viewResult.ViewData["CurrentFilter"]);
        Assert.Equal("UpdatedAt", currentFilter.SortBy);
        Assert.Equal("desc", currentFilter.SortDirection);
        Assert.Equal(1, currentFilter.Page);
        Assert.Equal(10, currentFilter.PageSize);
    }    /// <summary>
    /// 案例二：測試有條件篩選
    /// </summary>
    [Fact]
    public void List_WithStatusFilter_ShouldReturnOnlyPublishedArticles()
    {
        // Arrange - 建立篩選條件：只顯示已發布的文章
        var (controller, repository) = GetTestServices();
        var filter = new ArticleFilterDto
        {
            Status = "published",
            SortBy = "UpdatedAt",
            SortDirection = "desc"
        };

        // Act - 呼叫 List 方法
        var result = controller.List(filter);

        // Assert - 驗證結果
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<ArticleListItemViewModel>>(viewResult.Model);

        // 驗證只回傳已發布的文章（4篇，排除草稿狀態的 React Hooks 文章）
        Assert.Equal(4, model.Count);

        // 驗證所有回傳的文章都是已發布狀態
        Assert.All(model, article => Assert.Equal("published", article.Status));

        // 驗證排序順序
        var expectedTitles = new[]
        {
            "JavaScript ES6+ 新特性完整指南",
            "CSS Grid 與 Flexbox 完整比較", 
            "Node.js 效能優化實戰技巧",
            "MongoDB 資料庫設計最佳實踐"
        };

        for (int i = 0; i < expectedTitles.Length; i++)
        {
            Assert.Equal(expectedTitles[i], model[i].Title);
        }
    }

    /// <summary>
    /// 案例三：測試搜尋關鍵字篩選
    /// </summary>
    [Fact]
    public void List_WithSearchKeyword_ShouldReturnMatchingArticles()
    {
        // Arrange - 建立篩選條件：搜尋包含「資料庫」的文章
        var (controller, repository) = GetTestServices();
        var filter = new ArticleFilterDto
        {
            SearchKeyword = "資料庫",
            SortBy = "UpdatedAt",
            SortDirection = "desc"
        };

        // Act - 呼叫 List 方法
        var result = controller.List(filter);

        // Assert - 驗證結果
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<ArticleListItemViewModel>>(viewResult.Model);

        // 驗證只回傳包含「資料庫」關鍵字的文章
        Assert.Single(model);
        Assert.Equal("MongoDB 資料庫設計最佳實踐", model[0].Title);
        Assert.Equal("資料庫專家", model[0].Author);
    }

    /// <summary>
    /// 案例四：測試分類篩選
    /// </summary>    [Fact]
    public void List_WithCategoryFilter_ShouldReturnArticlesInCategory()
    {
        // Arrange - 建立篩選條件：只顯示前端開發分類（CategoryId = 1）的文章
        var (controller, repository) = GetTestServices();
        var filter = new ArticleFilterDto
        {
            CategoryId = 1,
            SortBy = "UpdatedAt",
            SortDirection = "desc"
        };

        // Act - 呼叫 List 方法
        var result = controller.List(filter);

        // Assert - 驗證結果
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<ArticleListItemViewModel>>(viewResult.Model);

        // 驗證只回傳前端開發分類的文章（3篇）
        Assert.Equal(3, model.Count);

        // 驗證所有文章都屬於前端開發分類
        Assert.All(model, article => Assert.Equal("前端開發", article.CategoryName));

        // 驗證排序順序
        var expectedTitles = new[]
        {
            "JavaScript ES6+ 新特性完整指南",
            "CSS Grid 與 Flexbox 完整比較",
            "React Hooks 深度解析"
        };

        for (int i = 0; i < expectedTitles.Length; i++)
        {
            Assert.Equal(expectedTitles[i], model[i].Title);
        }
    }

    /// <summary>
    /// 案例五：測試複合條件篩選
    /// </summary>    [Fact]
    public void List_WithMultipleFilters_ShouldReturnMatchingArticles()
    {
        // Arrange - 建立篩選條件：前端開發分類 + 已發布狀態
        var (controller, repository) = GetTestServices();
        var filter = new ArticleFilterDto
        {
            CategoryId = 1,
            Status = "published",
            SortBy = "Views",
            SortDirection = "desc"
        };

        // Act - 呼叫 List 方法
        var result = controller.List(filter);

        // Assert - 驗證結果
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<ArticleListItemViewModel>>(viewResult.Model);

        // 驗證只回傳符合條件的文章（前端開發 + 已發布 = 2篇，排除草稿狀態的 React Hooks）
        Assert.Equal(2, model.Count);

        // 驗證所有文章都符合篩選條件
        Assert.All(model, article =>
        {
            Assert.Equal("前端開發", article.CategoryName);
            Assert.Equal("published", article.Status);
        });

        // 驗證按瀏覽數降序排列
        Assert.True(model[0].Views >= model[1].Views);
        Assert.Equal("JavaScript ES6+ 新特性完整指南", model[0].Title); // 1250 views
        Assert.Equal("CSS Grid 與 Flexbox 完整比較", model[1].Title);    // 980 views
    }

    /// <summary>
    /// 案例六：測試排序功能
    /// </summary>
    [Fact]
    public void List_WithTitleSortAscending_ShouldReturnArticlesSortedByTitle()    {
        // Arrange - 建立篩選條件：按標題升序排列
        var (controller, repository) = GetTestServices();
        var filter = new ArticleFilterDto
        {
            SortBy = "Title",
            SortDirection = "asc"
        };

        // Act - 呼叫 List 方法
        var result = controller.List(filter);

        // Assert - 驗證結果
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<ArticleListItemViewModel>>(viewResult.Model);

        // 驗證按標題字母順序排列
        var expectedOrder = new[]
        {
            "CSS Grid 與 Flexbox 完整比較",
            "JavaScript ES6+ 新特性完整指南",
            "MongoDB 資料庫設計最佳實踐",
            "Node.js 效能優化實戰技巧",
            "React Hooks 深度解析"
        };

        Assert.Equal(expectedOrder.Length, model.Count);
        for (int i = 0; i < expectedOrder.Length; i++)
        {
            Assert.Equal(expectedOrder[i], model[i].Title);
        }
    }

    /// <summary>
    /// 案例七：測試空的搜尋結果
    /// </summary>    [Fact]
    public void List_WithNonExistentSearchKeyword_ShouldReturnEmptyList()
    {
        // Arrange - 建立篩選條件：搜尋不存在的關鍵字
        var (controller, repository) = GetTestServices();
        var filter = new ArticleFilterDto
        {
            SearchKeyword = "不存在的關鍵字XYZ123"
        };

        // Act - 呼叫 List 方法
        var result = controller.List(filter);

        // Assert - 驗證結果
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<ArticleListItemViewModel>>(viewResult.Model);

        // 驗證回傳空列表
        Assert.Empty(model);
    }
}
