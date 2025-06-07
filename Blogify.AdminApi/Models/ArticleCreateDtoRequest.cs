namespace Blogify.AdminApi.Models;

public class ArticleCreateDtoRequest
{
    public string Title { get; set; } = null!;
    public string? Summary { get; set; }
    public string? Author { get; set; } // 作者
    public int CategoryId { get; set; }
    public List<string>? TagNames { get; set; } // 標籤名稱陣列
    public int ReadingTimeMinutes { get; set; }
    public string? ImageUrl { get; set; }
    public int Status { get; set; } // ArticleStatus
    public bool IsFeatured { get; set; }
    public string Content { get; set; } = null!;
}

