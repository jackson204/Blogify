using Blogify.AdminApi.Models;
using Blogify.AdminApi.Models.Repository;
using Blogify.AdminApi.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Blogify.AdminApi.Controllers;

public class ArticleController : Controller
{
    private readonly ArticleRepository _articleRepository;

    public ArticleController(ArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public IActionResult List()
    {
        var viewModel = _articleRepository.GetArticles()
            .Select(article => new ArticleListItemViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Excerpt = article.Excerpt,
                Author = article.Author,
                CategoryName = article.Category?.Name,
                Tags = article.Tags,
                ReadTime = article.ReadTime,
                Image = article.Image,
                Status = article.Status,
                Featured = article.Featured,
                Views = article.Views,
                UpdatedAt = article.UpdatedAt
            })
            .ToList();

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
            CategoryId = model.Category,
            UpdatedAt = DateTime.Now
        };

        _articleRepository.Add(article);
        return RedirectToAction(nameof(List));
    }
}
