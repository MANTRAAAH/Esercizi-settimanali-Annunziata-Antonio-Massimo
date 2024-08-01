using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaS7.Context;
using PizzeriaS7.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaS7.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProdottiAdminController : Controller
    {
        private readonly PizzeriaContext _context;

        public ProdottiAdminController(PizzeriaContext context)
        {
            _context = context;
        }

        // GET: ProdottiAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _context.Prodotti.ToListAsync());
        }

        // GET: ProdottiAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodotto = await _context.Prodotti
                .Include(p => p.Ingredienti) // Include degli ingredienti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prodotto == null)
            {
                return NotFound();
            }

            return View(prodotto);
        }

        // GET: ProdottiAdmin/Create
        public IActionResult Create()
        {
            ViewBag.AllIngredienti = _context.Ingredienti.ToList(); // Passiamo gli ingredienti disponibili alla vista
            return View();
        }

        // POST: ProdottiAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,FotoUrl,Prezzo,TempoConsegna,IngredientiIds")] Prodotto prodotto)
        {
            if (ModelState.IsValid)
            {
                prodotto.Ingredienti = new List<Ingrediente>();
                if (prodotto.IngredientiIds != null)
                {
                    foreach (var ingredienteId in prodotto.IngredientiIds)
                    {
                        var ingrediente = await _context.Ingredienti.FindAsync(ingredienteId);
                        if (ingrediente != null)
                        {
                            prodotto.Ingredienti.Add(ingrediente); // Aggiungi l'ingrediente al prodotto
                        }
                    }
                }

                _context.Add(prodotto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AllIngredienti = _context.Ingredienti.ToList(); // Ricarica gli ingredienti in caso di errore
            return View(prodotto);
        }

        // GET: ProdottiAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodotto = await _context.Prodotti
                .Include(p => p.Ingredienti) // Include degli ingredienti
                .FirstOrDefaultAsync(p => p.Id == id);
            if (prodotto == null)
            {
                return NotFound();
            }

            prodotto.IngredientiIds = prodotto.Ingredienti.Select(i => i.Id).ToArray(); // Popola gli ID degli ingredienti
            ViewBag.AllIngredienti = _context.Ingredienti.ToList(); // Passiamo gli ingredienti disponibili alla vista
            return View(prodotto);
        }

        // POST: ProdottiAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,FotoUrl,Prezzo,TempoConsegna,IngredientiIds")] Prodotto prodotto)
        {
            if (id != prodotto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var prodottoToUpdate = await _context.Prodotti
                        .Include(p => p.Ingredienti) // Include degli ingredienti
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (prodottoToUpdate == null)
                    {
                        return NotFound();
                    }

                    prodottoToUpdate.Nome = prodotto.Nome;
                    prodottoToUpdate.FotoUrl = prodotto.FotoUrl;
                    prodottoToUpdate.Prezzo = prodotto.Prezzo;
                    prodottoToUpdate.TempoConsegna = prodotto.TempoConsegna;

                    // Aggiorna gli ingredienti
                    prodottoToUpdate.Ingredienti.Clear();
                    if (prodotto.IngredientiIds != null)
                    {
                        foreach (var ingredienteId in prodotto.IngredientiIds)
                        {
                            var ingrediente = await _context.Ingredienti.FindAsync(ingredienteId);
                            if (ingrediente != null)
                            {
                                prodottoToUpdate.Ingredienti.Add(ingrediente); // Aggiungi l'ingrediente aggiornato
                            }
                        }
                    }

                    _context.Update(prodottoToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdottoExists(prodotto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AllIngredienti = _context.Ingredienti.ToList(); // Ricarica gli ingredienti in caso di errore
            return View(prodotto);
        }

        // GET: ProdottiAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodotto = await _context.Prodotti
                .Include(p => p.Ingredienti) // Include degli ingredienti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prodotto == null)
            {
                return NotFound();
            }

            return View(prodotto);
        }

        // POST: ProdottiAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prodotto = await _context.Prodotti
                .Include(p => p.Ingredienti) // Include la relazione
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prodotto == null)
            {
                return NotFound();
            }

            // Rimuovi manualmente tutte le associazioni con gli ingredienti
            _context.RemoveRange(_context.Set<Dictionary<string, object>>("ProdottoIngredienti").Where(pi => (int)pi["ProdottoId"] == id));

            // Ora puoi rimuovere il prodotto
            _context.Prodotti.Remove(prodotto);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool ProdottoExists(int id)
        {
            return _context.Prodotti.Any(e => e.Id == id);
        }
    }
}
