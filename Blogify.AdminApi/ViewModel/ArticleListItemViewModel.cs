namespace Blogify.AdminApi.ViewModel;

public class ArticleListItemViewModel
{
    public string StatusString => Status switch
    {
        "published" => "已發佈",
        "draft" => "草稿",
        _ => "未知狀態"
    };

    public int? Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Excerpt { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string? CategoryName { get; set; }

    public string Tags { get; set; } = string.Empty;

    public int ReadTime { get; set; }

    public string Image { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public bool Featured { get; set; }

    public int Views { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string StatusCheck => Status == "published" ? "status-published" : "status-draft";
}
