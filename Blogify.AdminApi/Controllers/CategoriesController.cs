using Microsoft.AspNetCore.Mvc;

namespace Blogify.AdminApi.Controllers;

public class CategoriesController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
