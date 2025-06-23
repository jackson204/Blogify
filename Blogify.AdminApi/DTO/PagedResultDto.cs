namespace Blogify.AdminApi.DTO;

public class PagedResultDto<T>
{
    /// <summary>
    /// 資料列表
    /// </summary>
    public List<T> Data { get; set; } = new List<T>();
    
    /// <summary>
    /// 總筆數
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// 目前頁數
    /// </summary>
    public int CurrentPage { get; set; }
    
    /// <summary>
    /// 每頁筆數
    /// </summary>
    public int PageSize { get; set; }
    
    /// <summary>
    /// 總頁數
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    
    /// <summary>
    /// 是否有上一頁
    /// </summary>
    public bool HasPreviousPage => CurrentPage > 1;
    
    /// <summary>
    /// 是否有下一頁
    /// </summary>
    public bool HasNextPage => CurrentPage < TotalPages;
}