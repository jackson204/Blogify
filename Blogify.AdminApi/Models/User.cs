namespace Blogify.AdminApi.Models;

public class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!; // 如「管理員」

    public string Email { get; set; } = null!;

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
