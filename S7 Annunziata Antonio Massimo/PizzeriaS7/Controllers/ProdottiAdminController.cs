using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PizzeriaS7.Context;
using PizzeriaS7.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaS7.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProdottiAdminController : Controller
    {
        private readonly PizzeriaContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<ProdottiAdminController> _logger;

        public ProdottiAdminController(PizzeriaContext context, IWebHostEnvironment hostEnvironment, ILogger<ProdottiAdminController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        // GET: ProdottiAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _context.Prodotti.Include(p => p.Immagini).ToListAsync());
        }

        // GET: ProdottiAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodotto = await _context.Prodotti
                .Include(p => p.Ingredienti)
                .Include(p => p.Immagini) // Include delle immagini
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
            ViewBag.AllIngredienti = _context.Ingredienti.ToList();
            return View();
        }

        // POST: ProdottiAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Prezzo,TempoConsegna,IngredientiIds")] Prodotto prodotto, IFormFile immagineFile)
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
                            prodotto.Ingredienti.Add(ingrediente);
                        }
                    }
                }

                _context.Add(prodotto);
                await _context.SaveChangesAsync();

                // Gestione dell'immagine
                if (immagineFile != null)
                {
                    var uploads = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    var filePath = Path.Combine(uploads, immagineFile.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await immagineFile.CopyToAsync(fileStream);
                    }

                    var prodottoImmagine = new ProdottiImmagini
                    {
                        ProdottoId = prodotto.Id,
                        ImmagineUrl = "/images/" + immagineFile.FileName
                    };

                    _context.ProdottiImmagini.Add(prodottoImmagine);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewBag.AllIngredienti = _context.Ingredienti.ToList();
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
                .Include(p => p.Ingredienti)
                .Include(p => p.Immagini) // Include delle immagini
                .FirstOrDefaultAsync(p => p.Id == id);
            if (prodotto == null)
            {
                return NotFound();
            }

            prodotto.IngredientiIds = prodotto.Ingredienti.Select(i => i.Id).ToArray();
            ViewBag.AllIngredienti = _context.Ingredienti.ToList();
            return View(prodotto);
        }

        // POST: ProdottiAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Prezzo,TempoConsegna,IngredientiIds")] Prodotto prodotto, IFormFile immagineFile)
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
                        .Include(p => p.Ingredienti)
                        .Include(p => p.Immagini)
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (prodottoToUpdate == null)
                    {
                        return NotFound();
                    }

                    prodottoToUpdate.Nome = prodotto.Nome;
                    prodottoToUpdate.Prezzo = prodotto.Prezzo;
                    prodottoToUpdate.TempoConsegna = prodotto.TempoConsegna;

                    prodottoToUpdate.Ingredienti.Clear();
                    if (prodotto.IngredientiIds != null)
                    {
                        foreach (var ingredienteId in prodotto.IngredientiIds)
                        {
                            var ingrediente = await _context.Ingredienti.FindAsync(ingredienteId);
                            if (ingrediente != null)
                            {
                                prodottoToUpdate.Ingredienti.Add(ingrediente);
                            }
                        }
                    }

                    // Gestione dell'immagine
                    if (immagineFile != null && immagineFile.Length > 0)
                    {
                        var uploads = Path.Combine(_hostEnvironment.WebRootPath, "images");
                        var filePath = Path.Combine(uploads, immagineFile.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await immagineFile.CopyToAsync(fileStream);
                        }

                        var prodottoImmagine = new ProdottiImmagini
                        {
                            ProdottoId = prodottoToUpdate.Id,
                            ImmagineUrl = "/images/" + immagineFile.FileName
                        };

                        _context.ProdottiImmagini.Add(prodottoImmagine);
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

            // Log detailed ModelState errors
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    _logger.LogWarning("ModelState error in key '{Key}': {ErrorMessage}", key, error.ErrorMessage);
                }
            }

            ViewBag.AllIngredienti = _context.Ingredienti.ToList();
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
                .Include(p => p.Ingredienti)
                .Include(p => p.Immagini) // Include delle immagini
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
                .Include(p => p.Ingredienti)
                .Include(p => p.Immagini) // Include delle immagini
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prodotto == null)
            {
                return NotFound();
            }

            // Rimuovi manualmente tutte le associazioni con gli ingredienti
            _context.RemoveRange(_context.Set<Dictionary<string, object>>("ProdottoIngredienti").Where(pi => (int)pi["ProdottoId"] == id));

            // Rimuovi le immagini associate
            var immagini = _context.ProdottiImmagini.Where(pi => pi.ProdottoId == id).ToList();
            foreach (var immagine in immagini)
            {
                var filePath = Path.Combine(_hostEnvironment.WebRootPath, immagine.ImmagineUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.ProdottiImmagini.RemoveRange(immagini);

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
