using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blogify.AdminApi.Models;

namespace Blogify.AdminApi.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BlogContext _context;

    public HomeController(ILogger<HomeController> logger, BlogContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult TestDb()
    {
        try
        {
            var user = _context.Users.FirstOrDefault();
            if (user != null)
            {
                return Content($"DB 連線成功，第一位使用者名稱：{user.UserName}");
            }
            else
            {
                return Content("DB 連線成功，但沒有使用者資料。");
            }
        }
        catch (Exception ex)
        {
            return Content($"DB 連線失敗：{ex.Message}");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
