using System.ComponentModel.DataAnnotations;

namespace Blogify.AdminApi.Models;

public class Article
{
    public int? Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string? Excerpt { get; set; }

    public string Author { get; set; }

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
    public string Content { get; set; }

    public int Views { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public Category Category { get; set; } // 導覽屬性
}
