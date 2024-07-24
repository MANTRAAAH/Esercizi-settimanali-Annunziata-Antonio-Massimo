using GestoreAlbergo.Models;
using GestoreAlbergo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GestoreAlbergo.Controllers
{
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

            // Log the camera ID
            _logger.LogInformation("Prenotazione retrieved. Camera ID: {CameraId}", prenotazione.NumeroCamera);

            prenotazione.Camera = await _cameraService.GetCameraByNumeroAsync(prenotazione.NumeroCamera);

            if (prenotazione.Camera == null)
            {
                _logger.LogWarning("Camera with ID {CameraID} not found.", prenotazione.NumeroCamera);
                return NotFound();
            }

            var serviziAggiuntivi = (await _prenotazioneService.GetServiziAggiuntiviByPrenotazioneIdAsync(prenotazioneId)).ToList();

            // Format the pricing and totals
            foreach (var servizio in serviziAggiuntivi)
            {
                servizio.PrezzoFormatted = servizio.Prezzo.ToString("C", System.Globalization.CultureInfo.CurrentCulture);
                servizio.TotaleFormatted = (servizio.Prezzo * servizio.Quantita).ToString("C", System.Globalization.CultureInfo.CurrentCulture);
            }

            decimal totaleServiziAggiuntivi = serviziAggiuntivi.Sum(sa => sa.Prezzo * sa.Quantita);
            var totale = prenotazione.TariffaApplicata - prenotazione.Caparra + totaleServiziAggiuntivi;

            var model = new CheckoutViewModel
            {
                Prenotazione = prenotazione,
                ServiziAggiuntivi = serviziAggiuntivi,
                TotaleDaSaldare = totale,
                TotaleDaSaldareFormatted = totale.ToString("C", System.Globalization.CultureInfo.CurrentCulture)
            };

            return View(model);
        }

    }
}

