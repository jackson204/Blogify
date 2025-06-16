namespace Blogify.AdminApi.Views.ViewModel;

public class ArticleListItemViewModel
{
    public int? Id { get; set; }

    public string Title { get; set; }

    public string Excerpt { get; set; }

    public string Author { get; set; }

    public string? CategoryName { get; set; }

    public string Tags { get; set; }

    public int ReadTime { get; set; }

    public string Image { get; set; }

    public string Status { get; set; }

    public bool Featured { get; set; }

    public int Views { get; set; }

    public DateTime UpdatedAt { get; set; }
}