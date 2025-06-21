using Blogify.AdminApi.Models;
using Blogify.AdminApi.Models.Repository;
using Blogify.AdminApi.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Blogify.AdminApi.Controllers;

public class ArticleController : Controller
{
    public IActionResult List()
    {
        var articles = ArticleRepository.GetArticles();
        var categories = CategoryRepository.Categories();
        var viewModel = articles.Select(a => new ArticleListItemViewModel
        {
            Id = a.Id,
            Title = a.Title,
            Excerpt = a.Excerpt,
            Author = a.Author,
            CategoryName = categories.FirstOrDefault(c => c.Id == a.CategoryId)?.Name,
            Tags = a.Tags,
            ReadTime = a.ReadTime,
            Image = a.Image,
            Status = a.Status,
            Featured = a.Featured,
            Views = a.Views,
            UpdatedAt = a.UpdatedAt
        }).ToList();
        return View(viewModel);
    }
    public IActionResult Create()
    {
        var category = CategoryRepository.Categories();
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
            CategoryId = model.Category
        };
        return View();
    }
}