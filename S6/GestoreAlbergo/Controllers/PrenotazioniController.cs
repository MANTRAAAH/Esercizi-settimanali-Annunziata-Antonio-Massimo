using Microsoft.AspNetCore.Mvc;
using GestoreAlbergo.Models;
using GestoreAlbergo.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GestoreAlbergo.Controllers
{
    public class PrenotazioniController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly ICameraService _cameraService;
        private readonly IPrenotazioneService _prenotazioneService;
        private readonly ILogger<PrenotazioniController> _logger;

        public PrenotazioniController(IClienteService clienteService, ICameraService cameraService, IPrenotazioneService prenotazioneService, ILogger<PrenotazioniController> logger)
        {
            _clienteService = clienteService;
            _cameraService = cameraService;
            _prenotazioneService = prenotazioneService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var prenotazioni = await _prenotazioneService.GetAllAsync();
            foreach (var prenotazione in prenotazioni)
            {
                prenotazione.Cliente = await _clienteService.GetByCodiceFiscaleAsync(prenotazione.CodiceFiscale);
                prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.NumeroCamera);
            }

            return View(prenotazioni);
        }

        public async Task<IActionResult> Details(int id)
        {
            var prenotazione = await _prenotazioneService.GetByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }

            prenotazione.Cliente = await _clienteService.GetByCodiceFiscaleAsync(prenotazione.CodiceFiscale);
            prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.NumeroCamera);

            return View(prenotazione);
        }

        public async Task<IActionResult> Create()
        {
            var clienti = await _clienteService.GetAllClientiAsync();
            var camere = await _cameraService.GetAllCamerasAsync();

            var model = new PrenotazioneViewModel
            {
                Prenotazione = new Prenotazione(),
                Clienti = clienti.Select(c => new SelectListItem
                {
                    Value = c.CodiceFiscale,
                    Text = c.Nome + " " + c.Cognome
                }).ToList(),
                Camere = camere.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Numero.ToString()
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Prenotazione")] PrenotazioneViewModel model)
        {
            _logger.LogInformation("Create action called.");

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Model state is valid.");
                    model.Prenotazione.Anno = DateTime.Now.Year;
                    model.Prenotazione.NumeroProgressivo = await _prenotazioneService.GetNextProgressiveNumberAsync(model.Prenotazione.Anno);

                    _logger.LogInformation("Anno set to {Anno}, NumeroProgressivo set to {NumeroProgressivo}.", model.Prenotazione.Anno, model.Prenotazione.NumeroProgressivo);

                    model.Prenotazione.Cliente = await _clienteService.GetByCodiceFiscaleAsync(model.Prenotazione.CodiceFiscale);
                    model.Prenotazione.Camera = await _cameraService.GetCameraByIdAsync(model.Prenotazione.NumeroCamera);

                    await _prenotazioneService.CreateAsync(model.Prenotazione);
                    _logger.LogInformation("Prenotazione created successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating the prenotazione.");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the prenotazione.");
                }
            }
            else
            {
                _logger.LogWarning("Model state is invalid.");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("Model state error in {Key}: {ErrorMessage}", state.Key, error.ErrorMessage);
                    }
                }
            }

            var clienti = await _clienteService.GetAllClientiAsync();
            var camere = await _cameraService.GetAllCamerasAsync();

            model.Clienti = clienti.Select(c => new SelectListItem
            {
                Value = c.CodiceFiscale,
                Text = c.Nome + " " + c.Cognome
            }).ToList();
            model.Camere = camere.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Numero.ToString()
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            _logger.LogInformation("Search called with query: {Query}", query);
            var clienti = await _clienteService.SearchClientiAsync(query);
            _logger.LogInformation("Search results count: {Count}", clienti.Count());
            return Json(clienti.Select(c => new {
                id = c.Id,
                nome = c.Nome,
                cognome = c.Cognome,
                citta = c.Citta,
                email = c.Email,
                telefono = c.Telefono,
                cellulare = c.Cellulare,
                provincia = c.Provincia,
                cod_fisc = c.CodiceFiscale
            }));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var prenotazione = await _prenotazioneService.GetByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }

            var clienti = await _clienteService.GetAllClientiAsync();
            var camere = await _cameraService.GetAllCamerasAsync();

            var model = new PrenotazioneEditViewModel
            {
                Prenotazione = prenotazione,
                Clienti = new SelectList(clienti, "CodiceFiscale", "Nome", prenotazione.CodiceFiscale),
                Camere = new SelectList(camere, "Id", "Numero", prenotazione.NumeroCamera)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PrenotazioneEditViewModel model)
        {
            if (id != model.Prenotazione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _prenotazioneService.UpdateAsync(model.Prenotazione);
                return RedirectToAction(nameof(Index));
            }

            var camere = await _cameraService.GetAllCamerasAsync();
            model.Clienti = new SelectList(await _clienteService.GetAllClientiAsync(), "CodiceFiscale", "Nome", model.Prenotazione.CodiceFiscale);
            model.Camere = new SelectList(camere, "Id", "Numero", model.Prenotazione.NumeroCamera);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var prenotazione = await _prenotazioneService.GetByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            return View(prenotazione);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _prenotazioneService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
