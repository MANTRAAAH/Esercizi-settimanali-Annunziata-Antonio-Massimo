using Microsoft.AspNetCore.Mvc;
using GestoreAlbergo.Models;
using GestoreAlbergo.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GestoreAlbergo.Controllers
{
    [Authorize(Roles = "Admin,Dipendente")]
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
                if (prenotazione.CameraId.HasValue)
                {
                    prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.CameraId.Value);
                }
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
            if (prenotazione.CameraId.HasValue)
            {
                prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.CameraId.Value);
            }

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
        public async Task<IActionResult> Create([Bind("Prenotazione,CameraId")] PrenotazioneViewModel model)
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

                    // Log the camera ID being passed
                    _logger.LogInformation("Camera ID passed: {CameraId}", model.CameraId);

                    // Ensure the Camera exists
                    var camera = await _cameraService.GetCameraByIdAsync(model.CameraId);
                    if (camera == null)
                    {
                        _logger.LogWarning("Camera with ID {CameraId} not found.", model.CameraId);
                        ModelState.AddModelError("CameraId", "The selected camera does not exist.");
                        return View(model);
                    }

                    model.Prenotazione.Cliente = await _clienteService.GetByCodiceFiscaleAsync(model.Prenotazione.CodiceFiscale);
                    model.Prenotazione.CameraId = model.CameraId; // Imposta CameraId
                    model.Prenotazione.Camera = camera;

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
                Value = c.Id.ToString(), // Use Id for value
                Text = c.Numero.ToString() // Display Numero
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
            _logger.LogInformation("Edit GET action called with id: {Id}", id);

            var prenotazione = await _prenotazioneService.GetByIdAsync(id);
            if (prenotazione == null)
            {
                _logger.LogWarning("Prenotazione with id {Id} not found.", id);
                return NotFound();
            }

            var clienti = await _clienteService.GetAllClientiAsync();
            var camere = await _cameraService.GetAllCamerasAsync();

            var model = new PrenotazioneEditViewModel
            {
                Prenotazione = prenotazione,
                Clienti = new SelectList(clienti, "CodiceFiscale", "Nome", prenotazione.CodiceFiscale),
                Camere = new SelectList(camere, "Id", "Numero", prenotazione.CameraId)
            };

            _logger.LogInformation("Edit GET action completed for id: {Id}", id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PrenotazioneEditViewModel model)
        {
            _logger.LogInformation("Edit POST action called with id: {Id}", id);

            if (id != model.Prenotazione.Id)
            {
                _logger.LogWarning("Mismatched id in Edit POST action. Route id: {RouteId}, Model id: {ModelId}", id, model.Prenotazione.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Model state is valid. Updating prenotazione with id: {Id}", id);

                    // Ensure the Camera exists
                    var camera = await _cameraService.GetCameraByIdAsync(model.Prenotazione.CameraId.Value);
                    if (camera == null)
                    {
                        _logger.LogWarning("Camera with ID {CameraId} not found.", model.Prenotazione.CameraId);
                        ModelState.AddModelError("CameraId", "The selected camera does not exist.");
                        return View(model);
                    }

                    model.Prenotazione.Camera = camera;
                    model.Prenotazione.Cliente = await _clienteService.GetByCodiceFiscaleAsync(model.Prenotazione.CodiceFiscale);

                    await _prenotazioneService.UpdateAsync(model.Prenotazione);
                    _logger.LogInformation("Prenotazione with id {Id} updated successfully.", id);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating prenotazione with id: {Id}", id);
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the prenotazione.");
                }
            }
            else
            {
                _logger.LogWarning("Model state is invalid for prenotazione with id: {Id}", id);
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

            model.Clienti = new SelectList(clienti, "CodiceFiscale", "Nome", model.Prenotazione.CodiceFiscale);
            model.Camere = new SelectList(camere, "Id", "Numero", model.Prenotazione.CameraId);

            _logger.LogInformation("Edit POST action completed for id: {Id}", id);
            return View(model);
        }



        public async Task<IActionResult> Delete(int id)
        {
            var prenotazione = await _prenotazioneService.GetByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            await _prenotazioneService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
