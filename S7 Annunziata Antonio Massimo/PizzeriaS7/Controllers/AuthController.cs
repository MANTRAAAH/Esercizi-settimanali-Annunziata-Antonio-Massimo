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

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {Username} logged in successfully.", model.Username);
                return RedirectToAction("Index", "Home");
            }

            _logger.LogWarning("Invalid login attempt for user {Username}.", model.Username);
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Utente { UserName = model.Username, Email = model.Email, Nome = model.Nome };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Recupera il ruolo "User" dalla tabella AspNetRoles
                    var roleId = "185c9685-79e8-4ecb-a967-a4a45c5e8d31"; // ID del ruolo User

                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError("Error assigning role to user {Username}: {Error}", model.Username, error.Description);
                            ModelState.AddModelError(string.Empty, "Failed to assign user role.");
                        }
                        return View(model); // Ritorna la vista con l'errore se il ruolo non è stato assegnato
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User {Username} registered and logged in successfully.", model.Username);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Error registering user {Username}: {Error}", model.Username, error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }
    }
}
