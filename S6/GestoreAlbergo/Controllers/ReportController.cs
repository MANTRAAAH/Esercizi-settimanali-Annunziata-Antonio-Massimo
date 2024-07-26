using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GestoreAlbergo.Controllers
{
    public class ReportController : Controller
    {
        private readonly string _connectionString;

        public ReportController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalPrenotazioniPensioneCompleta()
        {
            int totalPrenotazioni = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT COUNT(*) AS TotalePrenotazioni FROM Prenotazioni WHERE Dettagli LIKE '%pensione completa%'";
                var command = new SqlCommand(query, connection);

                await connection.OpenAsync();
                totalPrenotazioni = (int)await command.ExecuteScalarAsync();
            }

            ViewData["TotalPrenotazioni"] = totalPrenotazioni;
            return View();
        }
    }
}
