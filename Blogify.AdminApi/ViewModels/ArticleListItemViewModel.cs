namespace Blogify.AdminApi.ViewModels;

public class ArticleListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string CategoryName { get; set; }
    public string Status { get; set; }
    public int Views { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsFeatured { get; set; }
}
