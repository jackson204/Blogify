using Blogify.AdminApi.DTO;

namespace Blogify.AdminApi.ViewModel
{
    /// <summary>
    /// 文章列表頁面的視圖模型，包含分頁和篩選資訊
    /// </summary>
    public class ArticleListViewModel
    {
        /// <summary>
        /// 文章列表
        /// </summary>
        public List<ArticleListItemViewModel> Articles { get; set; } = new();
        
        /// <summary>
        /// 當前篩選條件
        /// </summary>
        public ArticleFilterDto Filter { get; set; } = new();
        
        /// <summary>
        /// 總筆數
        /// </summary>
        public int TotalCount { get; set; }
        
        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }
        
        /// <summary>
        /// 是否有上一頁
        /// </summary>
        public bool HasPreviousPage => Filter.Page > 1;
        
        /// <summary>
        /// 是否有下一頁
        /// </summary>
        public bool HasNextPage => Filter.Page < TotalPages;
    }
}
