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

    public IActionResult Edit(int id)
    {
        var article = _articleRepository.GetArticleById(id);
        if (article == null)
        {
            return NotFound();
        }

        var viewModel = new ArticleViewModel
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Excerpt = article.Excerpt,
            Author = article.Author,
            Tags = article.Tags,
            ReadTime = article.ReadTime,
            Image = article.Image,
            Status = article.Status,
            Featured = article.Featured,
            Category = article.CategoryId
        };

        ViewBag.Categories = CategoryRepository.Categories();
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Edit(int id, ArticleViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var article = _articleRepository.GetArticleById(id);
        if (article == null)
        {
            return NotFound();
        }

        article.Title = model.Title;
        article.Content = model.Content;
        article.Excerpt = model.Excerpt;
        article.Author = model.Author;
        article.Tags = model.Tags;
        article.ReadTime = model.ReadTime;
        article.Image = model.Image;
        article.Status = model.Status;
        article.Featured = model.Featured;
        article.CategoryId = model.Category;
        article.UpdatedAt = DateTime.Now;

        _articleRepository.Update(article);
        return RedirectToAction(nameof(List));
    }
}
