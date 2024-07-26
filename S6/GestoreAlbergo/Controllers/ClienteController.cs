using GestoreAlbergo.Models;
using GestoreAlbergo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GestoreAlbergo.Controllers
{
    [Authorize(Roles = "Admin,Dipendente")]
    public class ClientiController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly IPrenotazioneService _prenotazioneService;
        private readonly ILogger<ClientiController> _logger;

        public ClientiController(IClienteService clienteService, IPrenotazioneService prenotazioneService, ILogger<ClientiController> logger)
        {
            _clienteService = clienteService;
            _prenotazioneService = prenotazioneService;
            _logger = logger;
        }

        // GET: Clienti
        public async Task<IActionResult> Index()
        {
            var clienti = await _clienteService.GetAllClientiAsync();
            return View(clienti);
        }

        // GET: Clienti/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // GET: Clienti/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clienti/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _clienteService.CreateAsync(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clienti/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Edit GET action called for Cliente ID: {Id}", id);

            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente with ID: {Id} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Cliente with ID: {Id} retrieved successfully", id);
            return View(cliente);
        }

        // POST: Clienti/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            _logger.LogInformation("Edit POST action called for Cliente ID: {Id}", id);

            if (id != cliente.Id)
            {
                _logger.LogWarning("Route ID: {RouteId} does not match Cliente ID: {ClienteId}", id, cliente.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Model state is valid for Cliente ID: {Id}", id);
                    await _clienteService.UpdateAsync(cliente);
                    _logger.LogInformation("Cliente with ID: {Id} updated successfully", id);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating Cliente with ID: {Id}", id);
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the cliente.");
                }
            }
            else
            {
                _logger.LogWarning("Model state is invalid for Cliente ID: {Id}", id);
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("Model state error in {Key}: {ErrorMessage}", state.Key, error.ErrorMessage);
                    }
                }
            }
            return View(cliente);
        }

        // GET: Clienti/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clienti/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            // Elimina prenotazioni associate al cliente
            var prenotazioni = await _prenotazioneService.GetPrenotazioniByCodiceFiscaleAsync(cliente.CodiceFiscale);
            foreach (var prenotazione in prenotazioni)
            {
                await _prenotazioneService.DeleteAsync(prenotazione.Id);
            }

            // Elimina il cliente
            await _clienteService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
