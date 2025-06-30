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
            // 登入成功，重導向到管理後台首頁
            return RedirectToAction("Index", "Home");
        }

        // 登入失敗
        ModelState.AddModelError("", "帳號或密碼錯誤");
        return View(model);
    }

    // POST: Auth/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        // 清除登入狀態 (目前簡化版本，只需重導向到登入頁)
        return RedirectToAction("Login");
    }
}
