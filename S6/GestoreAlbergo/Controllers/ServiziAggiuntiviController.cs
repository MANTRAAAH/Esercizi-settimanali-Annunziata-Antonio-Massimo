using GestoreAlbergo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestoreAlbergo.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace GestoreAlbergo.Controllers
{
    [Route("ServiziAggiuntivi")]
    public class ServiziAggiuntiviController : Controller
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly IClienteService _clienteService;
        private readonly IPrenotazioneService _prenotazioneService;
        private readonly IListaServiziAggiuntiviService _listaServiziAggiuntiviService;
        private readonly ILogger<ServiziAggiuntiviController> _logger;

        public ServiziAggiuntiviController(DatabaseHelper databaseHelper, IClienteService clienteService, IPrenotazioneService prenotazioneService, IListaServiziAggiuntiviService listaServiziAggiuntiviService, ILogger<ServiziAggiuntiviController> logger)
        {
            _databaseHelper = databaseHelper;
            _clienteService = clienteService;
            _prenotazioneService = prenotazioneService;
            _listaServiziAggiuntiviService = listaServiziAggiuntiviService;
            _logger = logger;
        }


        [Authorize(Roles = "Admin,Dipendente")]
        [HttpGet("CercaPrenotazione")]
        public IActionResult CercaPrenotazione()
        {
            _logger.LogInformation("Accessed CercaPrenotazione GET method.");
            return View();
        }



        [Authorize(Roles = "Admin,Dipendente")]
        [HttpPost("CercaPrenotazione")]
        public async Task<IActionResult> CercaPrenotazione(string codiceFiscale)
        {
            _logger.LogInformation("Accessed CercaPrenotazione POST method with CodiceFiscale: {CodiceFiscale}", codiceFiscale);
            if (string.IsNullOrEmpty(codiceFiscale))
            {
                _logger.LogError("CodiceFiscale is null or empty.");
                return BadRequest("CodiceFiscale is required.");
            }

            var prenotazioni = await _prenotazioneService.GetPrenotazioniByCodiceFiscaleAsync(codiceFiscale);
            _logger.LogInformation("Found {Count} prenotazioni for CodiceFiscale: {CodiceFiscale}", prenotazioni.Count(), codiceFiscale);
            return View("RisultatiPrenotazione", prenotazioni);
        }
        [Authorize(Roles = "Admin,Dipendente")]
        [HttpGet("AggiungiServizioAggiuntivo")]
        public async Task<IActionResult> AggiungiServizioAggiuntivo(int prenotazioneId)
        {
            _logger.LogInformation("Accessed AggiungiServizioAggiuntivo GET method with prenotazioneId: {PrenotazioneID}", prenotazioneId);

            var listaServizi = await _listaServiziAggiuntiviService.GetAllAsync();
            var model = new AggiungiServizioViewModel
            {
                PrenotazioneID = prenotazioneId,
                ListaServizi = listaServizi.Select(ls => new SelectListItem
                {
                    Value = ls.Id.ToString(),
                    Text = ls.NomeServizio
                }).ToList()
            };

            return View(model);
        }
        [Authorize(Roles = "Admin,Dipendente")]
        [HttpPost("AggiungiServizioAggiuntivo")]
        public IActionResult AggiungiServizioAggiuntivo(ServizioAggiuntivo servizio)
        {
            _logger.LogInformation("Accessed AggiungiServizioAggiuntivo POST method with prenotazioneId: {PrenotazioneID}", servizio.PrenotazioneID);

            using (var connection = _databaseHelper.GetConnection())
            {
                var command = new SqlCommand(
                    "INSERT INTO ServiziAggiuntivi (PrenotazioneID, Data, Quantita, ListaServizioID) VALUES (@PrenotazioneID, @Data, @Quantita, @ListaServizioID)",
                    connection);

                command.Parameters.AddWithValue("@PrenotazioneID", servizio.PrenotazioneID);
                command.Parameters.AddWithValue("@Data", servizio.Data);
                command.Parameters.AddWithValue("@Quantita", servizio.Quantita);
                command.Parameters.AddWithValue("@ListaServizioID", servizio.ListaServizioID);

                connection.Open();
                command.ExecuteNonQuery();
                _logger.LogInformation("Inserted ServizioAggiuntivo for prenotazioneId: {PrenotazioneID}", servizio.PrenotazioneID);
            }

            return RedirectToAction("CercaPrenotazione", "ServiziAggiuntivi", new { id = servizio.PrenotazioneID });
        }


        [Authorize(Roles = "Admin,Dipendente")]
        [HttpGet("ListaServiziAggiuntivi/{prenotazioneId}")]
        public async Task<IActionResult> ListaServiziAggiuntivi(int prenotazioneId)
        {
            _logger.LogInformation("Accessed ListaServiziAggiuntivi GET method for prenotazioneId: {PrenotazioneID}", prenotazioneId);

            var serviziAggiuntivi = await _prenotazioneService.GetServiziAggiuntiviByPrenotazioneIdAsync(prenotazioneId);
            var viewModel = serviziAggiuntivi.Select(sa => new ServizioAggiuntivoViewModel
            {
                ID = sa.ID,
                Descrizione = sa.Descrizione,
                Quantita = sa.Quantita,
                Prezzo = sa.Prezzo.ToString("C", System.Globalization.CultureInfo.CurrentCulture),
                Totale = (sa.Prezzo * sa.Quantita).ToString("C", System.Globalization.CultureInfo.CurrentCulture)
            }).ToList();

            ViewBag.PrenotazioneId = prenotazioneId;

            return View(viewModel);
        }


        [Authorize(Roles = "Admin,Dipendente")]
        [HttpPost("RimuoviServizioAggiuntivo")]
        public async Task<IActionResult> RimuoviServizioAggiuntivo(int servizioId, int prenotazioneId)
        {
            _logger.LogInformation("Accessed RimuoviServizioAggiuntivo POST method with servizioId: {ServizioID}", servizioId);

            using (var connection = _databaseHelper.GetConnection())
            {
                var command = new SqlCommand("DELETE FROM ServiziAggiuntivi WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", servizioId);
                connection.Open();
                await command.ExecuteNonQueryAsync();
                _logger.LogInformation("Deleted ServizioAggiuntivo with ID: {ID}", servizioId);
            }

            return RedirectToAction("ListaServiziAggiuntivi", new { prenotazioneId });
        }
        
        [Authorize(Roles = "Admin,Dipendente")]
        private async Task<ServizioAggiuntivo> GetServizioAggiuntivoByIdAsync(int id)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                var command = new SqlCommand("SELECT * FROM ServiziAggiuntivi WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
                connection.Open();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new ServizioAggiuntivo
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            PrenotazioneID = reader.GetInt32(reader.GetOrdinal("PrenotazioneID")),
                            Data = reader.GetDateTime(reader.GetOrdinal("Data")),
                            Quantita = reader.GetInt32(reader.GetOrdinal("Quantita")),
                            ListaServizioID = reader.GetInt32(reader.GetOrdinal("ListaServizioID"))
                        };
                    }
                }
            }

            return null;
        }
        [AllowAnonymous]
        [HttpGet("CatalogoServizi")]
        public async Task<IActionResult> CatalogoServizi()
        {
            _logger.LogInformation("Accessed ListaServiziAggiuntivi GET method.");
            var servizi = await _listaServiziAggiuntiviService.GetAllAsync();
            return View(servizi);
        }
    }
}
