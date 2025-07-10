namespace Blogify.ClientApi.ViewModel;

/// <summary>
/// 分類 ViewModel
/// </summary>
public class CategoryViewModel
{
    /// <summary>
    /// 分類 ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 分類名稱
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 分類描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 分類下的文章數量
    /// </summary>
    public int ArticleCount { get; set; }

    /// <summary>
    /// 分類 URL 路徑
    /// </summary>
    public string Slug { get; set; } = string.Empty;
}
