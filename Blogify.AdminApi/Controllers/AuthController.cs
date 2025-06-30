using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blogify.AdminApi.Controllers
{
    public class AuthController : Controller
    {
        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool rememberMe)
        {
        
            return View();
        }
    }
}
