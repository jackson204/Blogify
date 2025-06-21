using Blogify.AdminApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Blogify.AdminApi.Models.Repository;

public static class CategoryRepository
{
    public static List<Category> Categories()
    {
        using var context = new BlogContext(new DbContextOptionsBuilder<BlogContext>()
            .UseInMemoryDatabase("BlogDb")
            .Options);
        return context.Categories.ToList();
    }
}