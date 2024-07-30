using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaS7.Context;
using PizzeriaS7.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaS7.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdiniAdminController : Controller
    {
        private readonly PizzeriaContext _context;

        public OrdiniAdminController(PizzeriaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ordini = await _context.Ordini.Include(o => o.DettagliOrdine).ThenInclude(d => d.Prodotto).ToListAsync();
            return View(ordini);
        }

        [HttpPost]
        public async Task<IActionResult> MarcaComeEvaso(int id)
        {
            var ordine = await _context.Ordini.FindAsync(id);
            if (ordine == null)
            {
                return NotFound();
            }

            ordine.Evaso = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Report()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Report(DateTime data)
        {
            var ordiniEvasi = await _context.Ordini.Where(o => o.Evaso && o.DataOrdine.Date == data.Date).ToListAsync();
            var totaleIncasso = ordiniEvasi.Sum(o => o.DettagliOrdine.Sum(d => d.PrezzoTotale));
            ViewBag.NumeroOrdiniEvasi = ordiniEvasi.Count;
            ViewBag.TotaleIncasso = totaleIncasso;

            return View();
        }
    }
}
