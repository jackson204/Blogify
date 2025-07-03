using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blogify.AdminApi.Models;
using Blogify.AdminApi.Models.Repository;

namespace Blogify.AdminApi.Controllers;

[Authorize]
public class CategoriesController : Controller
{
    private readonly CategoryRepository _categoryRepository;

    public CategoriesController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    // GET: Categories
    public IActionResult Index(string? search = null, string? sort = null)
    {
        var categories = _categoryRepository.GetCategories();

        // 应用搜索过滤
        categories = ApplySearchFilter(categories, search);

        // 应用排序
        categories = ApplySorting(categories, sort);

        // 将查询条件传递给 View 以保持表单状态
        ViewBag.CurrentSearch = search;
        ViewBag.CurrentSort = sort;

        return View(categories);
    }

    /// <summary>
    /// 根据搜索条件过滤分类
    /// </summary>
    private List<Category> ApplySearchFilter(List<Category> categories, string? search)
    {
        if (string.IsNullOrEmpty(search))
            return categories;

        return categories.Where(c =>
            (c.Name?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (c.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
        ).ToList();
    }

    /// <summary>
    /// 根据排序条件对分类进行排序
    /// </summary>
    private List<Category> ApplySorting(List<Category> categories, string? sort)
    {
        return sort?.ToLower() switch
        {
            "count" => categories.OrderByDescending(c => c.Articles.Count).ToList(),
            "createdat" => categories.OrderByDescending(c => c.Id).ToList(), // 假设 Id 代表创建顺序
            "name" or null => categories.OrderBy(c => c.Name).ToList(),
            _ => categories.OrderBy(c => c.Name).ToList()
        };
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
