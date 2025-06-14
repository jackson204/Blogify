using Microsoft.AspNetCore.Mvc;

namespace Blogify.AdminApi.Controllers;


public class ArticleController : Controller
{
    public IActionResult Create()
    {
        return View();
    }
}
