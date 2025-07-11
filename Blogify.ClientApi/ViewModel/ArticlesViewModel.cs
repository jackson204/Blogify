namespace Blogify.ClientApi.ViewModel;

/// <summary>
/// 文章列表頁面的 ViewModel
/// </summary>
public class ArticlesViewModel
{
    /// <summary>
    /// 文章列表
    /// </summary>
    public List<HomeArticleViewModel> Articles { get; set; } = new();

    /// <summary>
    /// 總文章數
    /// </summary>
    public int TotalArticles { get; set; }

    /// <summary>
    /// 當前頁數
    /// </summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>
    /// 每頁文章數
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// 總頁數
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// 分類名稱（如果是分類頁面）
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// 分類 Slug（如果是分類頁面）
    /// </summary>
    public string? CategorySlug { get; set; }

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

    /// <summary>
    /// 是否為分類頁面
    /// </summary>
    public bool IsCategoryPage => !string.IsNullOrEmpty(CategoryName);

    /// <summary>
    /// 頁面標題
    /// </summary>
    public string PageTitle => IsCategoryPage ? $"{CategoryName} 分類文章" : "所有文章";

    /// <summary>
    /// 頁面描述
    /// </summary>
    public string PageDescription => IsCategoryPage 
        ? $"瀏覽 {CategoryName} 分類下的所有文章，共 {TotalArticles} 篇"
        : $"瀏覽所有文章，共 {TotalArticles} 篇文章";
}
