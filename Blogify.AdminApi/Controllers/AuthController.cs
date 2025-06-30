using Microsoft.AspNetCore.Mvc;
using Blogify.AdminApi.DTO;

namespace Blogify.AdminApi.Controllers;

public class AuthController : Controller
{
    // 假資料：僅供展示
    private const string DemoUsername = "admin";
    private const string DemoPassword = "password";

    // GET: Auth/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Auth/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.Username == DemoUsername && model.Password == DemoPassword)
        {
            // 登入成功，直接重導向到文章列表
            return RedirectToAction("List", "Article");
        }

        // 登入失敗
        ModelState.AddModelError("", "帳號或密碼錯誤");
        return View(model);
    }
}
