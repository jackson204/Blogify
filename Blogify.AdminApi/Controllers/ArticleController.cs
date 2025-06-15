using Blogify.AdminApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blogify.AdminApi.Controllers;

public class ArticleController : Controller
{
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
    public IActionResult Create(Article article)
    {
        return View();
    }
}
