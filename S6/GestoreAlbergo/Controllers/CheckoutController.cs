using GestoreAlbergo.Models;
using GestoreAlbergo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GestoreAlbergo.Controllers
{
    [Authorize(Roles = "Admin,Dipendente")]
    public class CheckoutController : Controller
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly IPrenotazioneService _prenotazioneService;
        private readonly ICameraService _cameraService;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(DatabaseHelper databaseHelper, IPrenotazioneService prenotazioneService, ILogger<CheckoutController> logger,ICameraService cameraService)
        {
            _databaseHelper = databaseHelper;
            _prenotazioneService = prenotazioneService;
            _logger = logger;
            _cameraService = cameraService;
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string codiceFiscale)
        {
            _logger.LogInformation("Accessed Search POST method with CodiceFiscale: {CodiceFiscale}", codiceFiscale);
            var prenotazioni = await _prenotazioneService.GetPrenotazioniByCodiceFiscaleAsync(codiceFiscale);
            _logger.LogInformation("Found {Count} prenotazioni for CodiceFiscale: {CodiceFiscale}", prenotazioni.Count(), codiceFiscale);

            return View("SearchResults", prenotazioni);
        }

        [HttpGet]
        public async Task<IActionResult> GetCheckoutDetails(int prenotazioneId)
        {
            var prenotazione = await _prenotazioneService.GetByIdAsync(prenotazioneId);
            if (prenotazione == null)
            {
                _logger.LogWarning("Prenotazione with ID {PrenotazioneID} not found.", prenotazioneId);
                return NotFound();
            }

            _logger.LogInformation("Prenotazione retrieved. Camera ID: {CameraId}", prenotazione.CameraId);

            if (prenotazione.CameraId.HasValue)
            {
                prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.CameraId.Value);
            }
            else
            {
                _logger.LogWarning("Camera ID is null for Prenotazione with ID {PrenotazioneID}.", prenotazioneId);
                return NotFound();
            }

            if (prenotazione.Camera == null)
            {
                _logger.LogWarning("Camera with ID {CameraID} not found.", prenotazione.CameraId);
                return NotFound();
            }

            var serviziAggiuntivi = (await _prenotazioneService.GetServiziAggiuntiviByPrenotazioneIdAsync(prenotazioneId)).ToList();

            foreach (var servizio in serviziAggiuntivi)
            {
                servizio.PrezzoFormatted = servizio.Prezzo.ToString("C", System.Globalization.CultureInfo.CurrentCulture);
                servizio.TotaleFormatted = (servizio.Prezzo * servizio.Quantita).ToString("C", System.Globalization.CultureInfo.CurrentCulture);
            }

            decimal totaleServiziAggiuntivi = serviziAggiuntivi.Sum(sa => sa.Prezzo * sa.Quantita);
            var totale = prenotazione.TariffaApplicata - prenotazione.Caparra + totaleServiziAggiuntivi;

            var cliente = await _prenotazioneService.GetClienteByPrenotazioneIdAsync(prenotazioneId);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente for Prenotazione with ID {PrenotazioneID} not found.", prenotazioneId);
                return NotFound();
            }

            var model = new CheckoutViewModel
            {
                Prenotazione = prenotazione,
                ServiziAggiuntivi = serviziAggiuntivi,
                TotaleDaSaldare = totale,
                TotaleDaSaldareFormatted = totale.ToString("C", System.Globalization.CultureInfo.CurrentCulture),
                Cliente = cliente
            };

            return View(model);
        }



    }
}

