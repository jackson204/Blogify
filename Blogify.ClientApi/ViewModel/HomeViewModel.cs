namespace Blogify.ClientApi.ViewModel;

/// <summary>
/// 首頁 ViewModel
/// </summary>
public class HomeViewModel
{
    /// <summary>
    /// 最新文章列表
    /// </summary>
    public List<HomeArticleViewModel> LatestArticles { get; set; } = new();

    /// <summary>
    /// 精選文章列表
    /// </summary>
    public List<HomeArticleViewModel> FeaturedArticles { get; set; } = new();

    /// <summary>
    /// 熱門文章列表
    /// </summary>
    public List<HomeArticleViewModel> PopularArticles { get; set; } = new();

    /// <summary>
    /// 分類列表
    /// </summary>
    public List<CategoryViewModel> Categories { get; set; } = new();

    /// <summary>
    /// 總文章數
    /// </summary>
    public int TotalArticles { get; set; }

    /// <summary>
    /// 總分類數
    /// </summary>
    public int TotalCategories { get; set; }

    /// <summary>
    /// 最新文章數量
    /// </summary>
    public int LatestArticlesCount { get; set; } = 10;

    /// <summary>
    /// 網站標題
    /// </summary>
    public string SiteTitle { get; set; } = "MyBlog";

    /// <summary>
    /// 網站描述
    /// </summary>
    public string SiteDescription { get; set; } = "專業的技術部落格平台，分享最新的程式設計知識和開發經驗";

    /// <summary>
    /// 歡迎訊息
    /// </summary>
    public string WelcomeMessage { get; set; } = "歡迎來到 MyBlog";

    // 分頁相關屬性
    /// <summary>
    /// 當前頁數
    /// </summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>
    /// 每頁文章數
    /// </summary>
    public int PageSize { get; set; } = 5;

    /// <summary>
    /// 總頁數
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// 是否有上一頁
    /// </summary>
    public bool HasPreviousPage => CurrentPage > 1;

    /// <summary>
    /// 是否有下一頁
    /// </summary>
    public bool HasNextPage => CurrentPage < TotalPages;

    /// <summary>
    /// 上一頁頁數
    /// </summary>
    public int PreviousPage => CurrentPage - 1;

    /// <summary>
    /// 下一頁頁數
    /// </summary>
    public int NextPage => CurrentPage + 1;
}
