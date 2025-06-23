using System.Text.Json;

namespace Blogify.AdminApi.Tests;

/// <summary>
/// 測試 Article API 端點的整合測試
/// </summary>
public class ArticleApiTests : TestBase
{
    /// <summary>
    /// 測試 GET /Article/GetArticlesApi 端點（預設值）
    /// </summary>
    [Fact]
    public async Task GetArticlesApi_WithDefaultValues_ShouldReturnAllArticles()
    {
        // Act - 呼叫 API 端點
        var response = await _client.GetAsync("/Article/GetArticlesApi");

        // Assert - 驗證回應
        response.EnsureSuccessStatusCode();
          var jsonString = await response.Content.ReadAsStringAsync();
        
        // 使用 JsonDocument 來檢查 JSON 結構，避免強型別反序列化問題
        using var document = JsonDocument.Parse(jsonString);
        var root = document.RootElement;
        
        // 驗證基本結構
        Assert.True(root.TryGetProperty("success", out var successElement));
        Assert.True(successElement.GetBoolean());
        
        Assert.True(root.TryGetProperty("data", out var dataElement));
        Assert.Equal(JsonValueKind.Array, dataElement.ValueKind);
        Assert.Equal(5, dataElement.GetArrayLength()); // 應該回傳所有 5 篇文章
        
        // 檢查第一筆資料的基本結構
        var firstItem = dataElement[0];
        Assert.True(firstItem.TryGetProperty("title", out _));
        Assert.True(firstItem.TryGetProperty("status", out _));
    }    /// <summary>
    /// 測試 GET /Article/GetArticlesApi 端點（有條件篩選）
    /// </summary>
    [Fact]
    public async Task GetArticlesApi_WithStatusFilter_ShouldReturnFilteredArticles()
    {        // Act - 呼叫 API 端點，只要已發布的文章
        var response = await _client.GetAsync("/Article/GetArticlesApi?Status=published");

        // Assert - 驗證回應
        response.EnsureSuccessStatusCode();
          var jsonString = await response.Content.ReadAsStringAsync();
        
        // 使用 JsonDocument 來檢查 JSON 結構
        using var document = JsonDocument.Parse(jsonString);
        var root = document.RootElement;
        
        // 驗證基本結構
        Assert.True(root.TryGetProperty("success", out var successElement));
        Assert.True(successElement.GetBoolean());
        
        Assert.True(root.TryGetProperty("data", out var dataElement));
        Assert.Equal(JsonValueKind.Array, dataElement.ValueKind);
        Assert.Equal(4, dataElement.GetArrayLength()); // 應該回傳 4 篇已發布的文章

        // 驗證所有文章都是已發布狀態
        foreach (var item in dataElement.EnumerateArray())
        {
            Assert.True(item.TryGetProperty("status", out var statusElement));
            Assert.Equal("published", statusElement.GetString());
        }
    }    /// <summary>
    /// 測試 GET /Article/GetArticlesApi 端點（搜尋關鍵字）
    /// </summary>
    [Fact]
    public async Task GetArticlesApi_WithSearchKeyword_ShouldReturnMatchingArticles()
    {        // Act - 呼叫 API 端點，搜尋包含「資料庫」的文章
        var response = await _client.GetAsync("/Article/GetArticlesApi?SearchKeyword=資料庫");

        // Assert - 驗證回應
        response.EnsureSuccessStatusCode();
          var jsonString = await response.Content.ReadAsStringAsync();
        
        // 使用 JsonDocument 來檢查 JSON 結構
        using var document = JsonDocument.Parse(jsonString);
        var root = document.RootElement;
        
        // 驗證基本結構
        Assert.True(root.TryGetProperty("success", out var successElement));
        Assert.True(successElement.GetBoolean());
        
        Assert.True(root.TryGetProperty("data", out var dataElement));
        Assert.Equal(JsonValueKind.Array, dataElement.ValueKind);
        Assert.Single(dataElement.EnumerateArray()); // 應該回傳 1 篇文章
        
        var firstItem = dataElement[0];
        Assert.True(firstItem.TryGetProperty("title", out var titleElement));
        Assert.Equal("MongoDB 資料庫設計最佳實踐", titleElement.GetString());
    }    /// <summary>
    /// 測試 GET /Article/GetArticlesApi 端點（分類篩選）
    /// </summary>
    [Fact]
    public async Task GetArticlesApi_WithCategoryFilter_ShouldReturnCategoryArticles()
    {        // Act - 呼叫 API 端點，篩選前端開發分類（CategoryId = 1）
        var response = await _client.GetAsync("/Article/GetArticlesApi?CategoryId=1");

        // Assert - 驗證回應
        response.EnsureSuccessStatusCode();
          var jsonString = await response.Content.ReadAsStringAsync();
        
        // 使用 JsonDocument 來檢查 JSON 結構
        using var document = JsonDocument.Parse(jsonString);
        var root = document.RootElement;
        
        // 驗證基本結構
        Assert.True(root.TryGetProperty("success", out var successElement));
        Assert.True(successElement.GetBoolean());
        
        Assert.True(root.TryGetProperty("data", out var dataElement));
        Assert.Equal(JsonValueKind.Array, dataElement.ValueKind);
        Assert.Equal(3, dataElement.GetArrayLength()); // 應該回傳 3 篇前端開發文章

        // 驗證所有文章都屬於前端開發分類
        foreach (var item in dataElement.EnumerateArray())
        {
            Assert.True(item.TryGetProperty("categoryName", out var categoryElement));
            Assert.Equal("前端開發", categoryElement.GetString());
        }
    }    /// <summary>
    /// 測試 GET /Article/GetArticlesApi 端點（複合條件）
    /// </summary>
    [Fact]
    public async Task GetArticlesApi_WithMultipleFilters_ShouldReturnMatchingArticles()
    {        // Act - 呼叫 API 端點，前端開發分類 + 已發布狀態
        var response = await _client.GetAsync("/Article/GetArticlesApi?CategoryId=1&Status=published");

        // Assert - 驗證回應
        response.EnsureSuccessStatusCode();
          var jsonString = await response.Content.ReadAsStringAsync();
        
        // 使用 JsonDocument 來檢查 JSON 結構
        using var document = JsonDocument.Parse(jsonString);
        var root = document.RootElement;
        
        // 驗證基本結構
        Assert.True(root.TryGetProperty("success", out var successElement));
        Assert.True(successElement.GetBoolean());
        
        Assert.True(root.TryGetProperty("data", out var dataElement));
        Assert.Equal(JsonValueKind.Array, dataElement.ValueKind);
        Assert.Equal(2, dataElement.GetArrayLength()); // 應該回傳 2 篇文章

        // 驗證所有文章都符合條件
        foreach (var item in dataElement.EnumerateArray())
        {
            Assert.True(item.TryGetProperty("categoryName", out var categoryElement));
            Assert.Equal("前端開發", categoryElement.GetString());
            
            Assert.True(item.TryGetProperty("status", out var statusElement));
            Assert.Equal("published", statusElement.GetString());
        }
    }
}

/// <summary>
/// API 回應的資料結構
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<ArticleItem> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}

/// <summary>
/// 文章項目的資料結構
/// </summary>
public class ArticleItem
{
    public int? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public string Tags { get; set; } = string.Empty;
    public int ReadTime { get; set; }
    public string? Image { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool Featured { get; set; }
    public int Views { get; set; }
    public DateTime UpdatedAt { get; set; }
}
