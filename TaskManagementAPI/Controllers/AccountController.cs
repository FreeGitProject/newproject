using Microsoft.AspNetCore.Mvc;

namespace TaskManagementAPI.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(); // returns Views/Account/Login.cshtml
        }
    }
}
