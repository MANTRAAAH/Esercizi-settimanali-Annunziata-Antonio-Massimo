using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzeriaS7.Models;
using System.Threading.Tasks;

namespace PizzeriaS7.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<Utente> _userManager;
        private readonly SignInManager<Utente> _signInManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<Utente> userManager, SignInManager<Utente> signInManager, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet("auth/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("auth/login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }

        [HttpGet("auth/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("auth/register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new Utente
            {
                UserName = model.Username,
                Email = model.Email,
                Nome = model.Nome,
                Ruolo = "User" // Imposta il ruolo predefinito a "User"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("User registered successfully.");
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpPost("auth/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("auth/accessdenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
