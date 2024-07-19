using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GestioneContravvenzioni.Models;
using System.Data.SqlClient;

public class AnagraficaController : Controller
{
    private readonly AnagraficaDAL _anagraficaDAL;

    public AnagraficaController(AnagraficaDAL anagraficaDAL)
    {
        _anagraficaDAL = anagraficaDAL;
    }

    public async Task<IActionResult> Index()
    {
        var anagrafiche = await _anagraficaDAL.GetAnagraficheAsync();
        return View(anagrafiche);
    }

    // GET: Anagrafica/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Anagrafica/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Cognome,Nome,Indirizzo,Città,CAP,Cod_Fisc")] Anagrafica anagrafica)
    {
        if (ModelState.IsValid)
        {
            await _anagraficaDAL.CreateAnagraficaAsync(anagrafica);
            return RedirectToAction(nameof(Index));
        }
        return View(anagrafica);
    }

    // GET: Anagrafica/Edit/Id
    public async Task<IActionResult> Edit(int id)
    {
        var anagrafica = await _anagraficaDAL.GetAnagraficaByIdAsync(id);
        if (anagrafica == null)
        {
            return NotFound();
        }
        return View(anagrafica);
    }

    // POST: Anagrafica/Edit/Id
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Idanagrafica,Cognome,Nome,Indirizzo,Città,CAP,Cod_Fisc")] Anagrafica anagrafica)
    {
        if (id != anagrafica.Idanagrafica)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _anagraficaDAL.UpdateAnagraficaAsync(anagrafica);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Errore durante l'aggiornamento: {ex.Message}");
            }
        }
        return View(anagrafica);
    }
}
