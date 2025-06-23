using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Blogify.AdminApi.Controllers;
using Blogify.AdminApi.DTO;
using Blogify.AdminApi.Models.Repository;
using Blogify.AdminApi.ViewModel;

namespace Blogify.AdminApi.Tests;

/// <summary>
/// 簡化的 ArticleController List 方法測試
/// </summary>
public class SimpleArticleListTests : TestBase
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
    public void Test_List_DefaultValues_ReturnsCorrectCount()
    {
        // Arrange - 不傳入任何篩選條件
        var (controller, repository) = GetTestServices();
        ArticleFilterDto? filter = null;

        // Act - 呼叫 List 方法
        var result = controller.List(filter);

        // Assert - 驗證結果
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<ArticleListItemViewModel>>(viewResult.Model);

        // 調試：輸出實際的數量
        var actualCount = model.Count;
        var actualTitles = string.Join(", ", model.Select(m => m.Title));
        
        // 驗證回傳的文章數量
        Assert.True(actualCount > 0, $"Expected at least 1 article, but got {actualCount}. Titles: {actualTitles}");
    }

    /// <summary>
    /// 案例二：測試有條件篩選 - 狀態篩選
    /// </summary>
    [Fact]
    public void Test_List_StatusFilter_ReturnsCorrectArticles()    {
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

        // 調試：輸出實際的結果
        var actualCount = model.Count;
        var publishedCount = model.Count(m => m.Status == "published");
        var allStatuses = string.Join(", ", model.Select(m => $"{m.Title}({m.Status})"));
          // 驗證有回傳文章且都是已發布狀態
        Assert.True(actualCount > 0, $"Expected at least 1 published article, but got {actualCount}. Articles: {allStatuses}");
        Assert.True(actualCount == publishedCount, $"All articles should be published. Expected {actualCount} published, got {publishedCount}. Articles: {allStatuses}");
    }

    /// <summary>
    /// 調試方法：檢查資料庫中的實際資料
    /// </summary>
    [Fact]
    public void Debug_CheckDatabaseContent()    {
        // 直接從 repository 取得資料
        var (controller, repository) = GetTestServices();
        var allArticles = repository.GetArticles().ToList();
        var articleInfo = string.Join("\n", allArticles.Select(a => 
            $"ID: {a.Id}, Title: {a.Title}, Status: {a.Status}, CategoryId: {a.CategoryId}"));
        
        // 輸出到測試結果
        Assert.True(allArticles.Count > 0, $"Database should contain articles. Found:\n{articleInfo}");
    }
}
