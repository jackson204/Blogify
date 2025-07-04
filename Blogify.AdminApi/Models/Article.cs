using System.ComponentModel.DataAnnotations;

namespace Blogify.AdminApi.Models;

public class Article
{
    public int? Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Excerpt { get; set; }

    public string Author { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }
    
    public string? Tags { get; set; }

    [Range(1, int.MaxValue)]
    public int ReadTime { get; set; } = 5;

    [Url]
    public string? Image { get; set; }

    public string Status { get; set; } = "draft";

    public bool Featured { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public int Views { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public Category Category { get; set; } = null!; // 導覽屬性
}
