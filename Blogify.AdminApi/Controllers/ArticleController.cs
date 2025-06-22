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

        ViewBag.Categories = CategoryRepository.Categories();

        return View(viewModel);
    }

    public IActionResult Create()
    {
        ViewBag.Action = nameof(Create);
        ViewBag.Categories = CategoryRepository.Categories();
        return View(new ArticleViewModel());
    }

    [HttpPost]
    public IActionResult Create(ArticleViewModel model)
    {
        var article = new Article()
        {
            Title = model.Title,
            Content = model.Content,
            Excerpt = model.Excerpt ?? string.Empty, // 避免 Excerpt 為 null
            Author = model.Author ?? string.Empty,   // 修正：避免 Author 為 null
            Tags = model.Tags ?? string.Empty,       // 避免 Tags 為 null
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

        ViewBag.Action = nameof(Edit);
        ViewBag.Categories = CategoryRepository.Categories();

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
        article.Excerpt = model.Excerpt ?? string.Empty; // 避免 Excerpt 為 null
        article.Author = model.Author ?? string.Empty;   // 修正：避免 Author 為 null
        article.Tags = model.Tags ?? string.Empty;       // 避免 Tags 為 null
        article.ReadTime = model.ReadTime;
        article.Image = model.Image;
        article.Status = model.Status;
        article.Featured = model.Featured;
        article.CategoryId = model.Category;
        article.UpdatedAt = DateTime.Now;

        _articleRepository.Update(article);
        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var article = _articleRepository.GetArticleById(id);
        if (article == null)
        {
            return NotFound();
        }

        _articleRepository.Delete(article);
        return RedirectToAction(nameof(List));
    }
}
