using Blogify.AdminApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Blogify.AdminApi.Models.Repository;

public class CategoryRepository
{
    private readonly BlogContext _context;

    public CategoryRepository(BlogContext context)
    {
        _context = context;
    }

    public List<Category> GetCategories()
    {
        return _context.Categories.ToList();
    }

    public Category? GetCategoryById(int id)
    {
        return _context.Categories.FirstOrDefault(c => c.Id == id);
    }

    public void Add(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }

    public void Delete(Category category)
    {
        _context.Categories.Remove(category);
        _context.SaveChanges();
    }
}