using GestoreAlbergo.Models;
using GestoreAlbergo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestoreAlbergo.Controllers
{
    [Authorize(Roles = "Admin,Dipendente")]
    public class ClientiController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClientiController(IClienteService clienteService)
        {
            _clienteService = clienteService;
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
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clienti/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _clienteService.UpdateAsync(cliente);
                return RedirectToAction(nameof(Index));
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clienteService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
