using System.ComponentModel.DataAnnotations;

namespace Blogify.AdminApi.ViewModels
{
    public class ArticleCreateViewModel
    {
        [Required(ErrorMessage = "標題為必填")]        
        public string Title { get; set; }

        public string Summary { get; set; }

        public string Author { get; set; }

        [Required(ErrorMessage = "分類為必選")]        
        public int? CategoryId { get; set; }

        public string TagNames { get; set; } // 逗號分隔

        public int? ReadingTimeMinutes { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string Status { get; set; }

        public bool IsFeatured { get; set; }

        [Required(ErrorMessage = "內容為必填")]        
        public string Content { get; set; }
    }
}

