namespace Blogify.AdminApi.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!; // 如「前端開發」「後端開發」

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
