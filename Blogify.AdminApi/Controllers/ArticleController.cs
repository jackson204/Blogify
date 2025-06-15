using Blogify.AdminApi.Models;
using Blogify.AdminApi.Models.Repository;
using Blogify.AdminApi.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blogify.AdminApi.Controllers;

public class ArticleController : Controller
{
    public IActionResult List()
    {
        var articles = ArticleRepository.GetArticles();
        return View(articles);
    }
    public IActionResult Create()
    {
        var category = new List<Category>
        {
            new() { Id = 1, Name = "前端開發" },
            new() { Id = 2, Name = "後端開發" },
            new() { Id = 3, Name = "資料庫" },
            new() { Id = 4, Name = "DevOps" },
            new() { Id = 5, Name = "行動開發" }
        };
        return View(category);
    }

    [HttpPost]
    public IActionResult Create(ArticleViewModel model)
    {
        var article = new Article()
        {
            Title = model.Title,
            Content = model.Content,
            Excerpt = model.Excerpt,
            Author = model.Author,
            Tags = model.Tags,
            ReadTime = model.ReadTime,
            Image = model.Image,
            Status = model.Status,
            Featured = model.Featured,
            Category = model.Category
        };
        return View();
    }
}
