using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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
        // 如果已經登入，重導向到首頁
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        
        return View();
    }

    // POST: Auth/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.Username == DemoUsername && model.Password == DemoPassword)
        {
            // 建立使用者聲明 (Claims)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("DisplayName", "管理員"),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            // 建立身分識別
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // 記住登入狀態
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            // 登入使用者
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), authProperties);

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
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        // 登出使用者
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
