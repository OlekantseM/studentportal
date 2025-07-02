using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace StudentPortal.Controllers
{
    public class AdminAuthController : Controller
    {
        private const string AdminUsername = "admin";  // Replace with your admin username
        private const string AdminPassword = "password"; // Replace with your secure admin password

        // GET: AdminAuth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: AdminAuth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string? username, string? password)
        {
            username = username?.Trim();
            password = password?.Trim();

            if (username == AdminUsername && password == AdminPassword)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Students", "Admin");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }


        // GET: AdminAuth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Login");
        }
    }
}
