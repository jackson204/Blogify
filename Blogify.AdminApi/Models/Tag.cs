namespace Blogify.AdminApi.Models;

public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!; // 如「組圖」「劍圖」「程式碼」

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
