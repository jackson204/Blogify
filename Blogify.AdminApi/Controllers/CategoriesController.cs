using Microsoft.AspNetCore.Mvc;
using Blogify.AdminApi.Models;
using Blogify.AdminApi.Models.Repository;

namespace Blogify.AdminApi.Controllers;

public class CategoriesController : Controller
{
    private readonly CategoryRepository _categoryRepository;

    public CategoriesController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    // GET
    public IActionResult Index()
    {
        var categories = _categoryRepository.GetCategories();
        return View(categories);
    }

    // GET: Categories/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Categories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category category)
    {
        if (ModelState.IsValid)
        {
            _categoryRepository.Add(category);
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }
    
    // GET: Categories/Edit/5
    public IActionResult Edit(int id)
    {
        var category = _categoryRepository.GetCategoryById(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    // POST: Categories/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Category category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _categoryRepository.Update(category);
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    // POST: Categories/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var category = _categoryRepository.GetCategoryById(id);
        if (category == null)
        {
            return NotFound();
        }

        _categoryRepository.Delete(category);
        return RedirectToAction(nameof(Index));
    }
}
