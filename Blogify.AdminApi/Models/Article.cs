namespace Blogify.AdminApi.Models;

public class Article
{
    public int Id { get; set; }

    public string Title { get; set; } = null!; // 文章標題

    public string? Summary { get; set; } // 文章摘要

    public string? Author { get; set; } // 作者 ID

    public int CategoryId { get; set; } // 分類 ID

    public ICollection<Tag> Tags { get; set; } = new List<Tag>(); // 多標籤
    
    public int ReadingTimeMinutes { get; set; }                   // 新增的屬性

    public string? ImageUrl { get; set; } // 文章圖片 URL

    public int Views { get; set; } // 瀏覽次數

    public ArticleStatus Status { get; set; } // 草稿 / 已發布

    public bool IsFeatured { get; set; } // 是否為精選文章

    public string Content { get; set; } = null!;                  // 文章內容（Markdown 或 HTML）

    public Category Category { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 
}

