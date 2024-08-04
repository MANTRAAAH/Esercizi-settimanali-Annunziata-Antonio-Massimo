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
            var ordini = await _context.Ordini
                .Include(o => o.DettagliOrdine)
                .ThenInclude(d => d.Prodotto)
                .Include(o => o.Utente) 
                .ToListAsync();

            var ordineViewModels = ordini.Select(o => new OrdineViewModel
            {
                OrdineId = o.Id,
                DataOrdine = o.DataOrdine,
                NomeUtente = o.Utente.UserName,
                Evaso = o.Evaso,
                DettagliOrdine = o.DettagliOrdine.Select(d => new DettaglioOrdineViewModel
                {
                    ProdottoNome = d.Prodotto.Nome,
                    Quantità = d.Quantità,
                    PrezzoTotale = d.PrezzoTotale
                }).ToList()
            }).ToList();

            return View(ordineViewModels);
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
        public async Task<IActionResult> Details(int id)
        {
            var ordine = await _context.Ordini
                .Include(o => o.DettagliOrdine)
                    .ThenInclude(d => d.Prodotto)
                .Include(o => o.Utente) 
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordine == null)
            {
                return NotFound();
            }

            var ordineViewModel = new OrdineViewModel
            {
                OrdineId = ordine.Id,
                DataOrdine = ordine.DataOrdine,
                NomeUtente = ordine.Utente.Nome,
                IndirizzoSpedizione = ordine.IndirizzoSpedizione, 
                Note = ordine.Note, 
                Evaso = ordine.Evaso,
                DettagliOrdine = ordine.DettagliOrdine.Select(d => new DettaglioOrdineViewModel
                {
                    ProdottoNome = d.Prodotto.Nome,
                    Quantità = d.Quantità,
                    PrezzoTotale = d.PrezzoTotale
                }).ToList()
            };

            return View(ordineViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Report(DateTime data)
        {
            // Includi i dettagli dell'ordine
            var ordiniEvasi = await _context.Ordini
                .Where(o => o.Evaso && o.DataOrdine.Date == data.Date)
                .Include(o => o.DettagliOrdine) // Include dei dettagli dell'ordine
                .ToListAsync();

            // Calcola il totale incasso
            var totaleIncasso = ordiniEvasi.Sum(o => o.DettagliOrdine.Sum(d => d.PrezzoTotale));

            // Assegna i valori a ViewBag per visualizzarli nella vista
            ViewBag.NumeroOrdiniEvasi = ordiniEvasi.Count;
            ViewBag.TotaleIncasso = totaleIncasso;

            return View();
        }

    }
}
