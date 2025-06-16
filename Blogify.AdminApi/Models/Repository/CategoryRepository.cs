namespace Blogify.AdminApi.Models.Repository;

public static class CategoryRepository
{
    public static List<Category> Categories()
    {
        var category = new List<Category>
        {
            new() { Id = 1, Name = "前端開發" },
            new() { Id = 2, Name = "後端開發" },
            new() { Id = 3, Name = "資料庫" },
            new() { Id = 4, Name = "DevOps" },
            new() { Id = 5, Name = "行動開發" }
        };
        return category;
    }
}