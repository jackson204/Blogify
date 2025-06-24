namespace Blogify.AdminApi.DTO;

public class ArticleFilterDto
{
    /// <summary>
    /// 搜尋關鍵字（標題、內容、作者）
    /// </summary>
    public string? SearchKeyword { get; set; }
    
    /// <summary>
    /// 文章狀態篩選
    /// </summary>
    public string? Status { get; set; }
    
    /// <summary>
    /// 分類 ID 篩選
    /// </summary>
    public int? CategoryId { get; set; }
    
    /// <summary>
    /// 排序欄位
    /// </summary>
    public string SortBy { get; set; } = "UpdatedAt";
    
    /// <summary>
    /// 排序方向 (asc/desc)
    /// </summary>
    public string SortDirection { get; set; } = "desc";
    
    /// <summary>
    /// 頁數
    /// </summary>
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// 每頁資料筆數
    /// </summary>
    public int PageSize { get; set; } = 10;
}
