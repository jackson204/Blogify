namespace Blogify.ClientApi.ViewModel;

/// <summary>
/// 首頁文章顯示的 ViewModel
/// </summary>
public class HomeArticleViewModel
{
    /// <summary>
    /// 文章 ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 文章標題
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 文章摘要
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// 文章內容預覽
    /// </summary>
    public string Preview { get; set; } = string.Empty;

    /// <summary>
    /// 發佈日期
    /// </summary>
    public DateTime PublishedDate { get; set; }

    /// <summary>
    /// 分類名稱
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>
    /// 作者名稱
    /// </summary>
    public string AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// 閱讀次數
    /// </summary>
    public int ViewCount { get; set; }

    /// <summary>
    /// 文章 URL 路徑
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// 文章標籤
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// 是否為精選文章
    /// </summary>
    public bool IsFeatured { get; set; }
}
