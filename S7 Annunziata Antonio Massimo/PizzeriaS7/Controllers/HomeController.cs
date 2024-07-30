using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzeriaS7.Models;
using System.Diagnostics;
using System.Text;

namespace PizzeriaS7.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<Utente> _userManager;

        public HomeController(UserManager<Utente> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    ViewBag.UserRole = "Admin";
                }
                else
                {
                    ViewBag.UserRole = "User";
                }
            }
            else
            {
                ViewBag.UserRole = null;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
