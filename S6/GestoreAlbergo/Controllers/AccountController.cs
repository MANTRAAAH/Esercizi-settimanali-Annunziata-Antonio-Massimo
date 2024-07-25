using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using GestoreAlbergo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace GestoreAlbergo.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand("SELECT * FROM Dipendenti WHERE NomeUtente = @NomeUtente", connection);
                    command.Parameters.AddWithValue("@NomeUtente", model.Username);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            var storedHash = reader["PasswordHash"].ToString();
                            var ruolo = reader["Ruolo"].ToString();

                            if (VerifyPasswordHash(model.Password, storedHash))
                            {
                                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.Username),
                            new Claim(ClaimTypes.Role, ruolo)
                        };

                                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "Tentativo di login non valido.");
            }

            return View(model);
        }


        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand("INSERT INTO Dipendenti (NomeUtente, PasswordHash, Ruolo) VALUES (@NomeUtente, @PasswordHash, @Ruolo)", connection);
                    command.Parameters.AddWithValue("@NomeUtente", model.Username);
                    command.Parameters.AddWithValue("@PasswordHash", HashPassword(model.Password));
                    command.Parameters.AddWithValue("@Ruolo", "User"); // Puoi cambiare il ruolo come necessario

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }


        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }

    }
}
