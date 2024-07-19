using GestioneContravvenzioni.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;


public class VerbaleController : Controller
{
    private readonly VerbaleDAL _verbaleDAL;
    private readonly AnagraficaDAL _anagraficaDAL;
    private readonly TipoViolazioneDAL _tipoViolazioneDAL;
    private readonly ILogger<VerbaleController> _logger;

    public VerbaleController(VerbaleDAL verbaleDAL, AnagraficaDAL anagraficaDAL, TipoViolazioneDAL tipoViolazioneDAL)
    {
        _verbaleDAL = verbaleDAL;
        _anagraficaDAL = anagraficaDAL;
        _tipoViolazioneDAL = tipoViolazioneDAL;
    }

    // GET: Restituisce la lista dei verbali
    public async Task<IActionResult> Index()
    {
        var verbali = await _verbaleDAL.GetVerbaliAsync();
        return View(verbali);
    }


    // GET: Verbale/Create, restituisce la vista per la creazione di un nuovo verbale
    public async Task<IActionResult> Create()
    {
        var anagrafiche = await _anagraficaDAL.GetAnagraficheAsync();
        var tipoViolazioni = await _tipoViolazioneDAL.GetTipiViolazioneAsync();

        var anagraficaList = new SelectList(anagrafiche.Select(a => new
        {
            a.Idanagrafica,
            DisplayText = $"{a.Nome} {a.Cognome}"
        }), "Idanagrafica", "DisplayText");

        var tipoViolazioneList = new SelectList(tipoViolazioni, "Idviolazione", "Descrizione");

        var viewModel = new VerbaleCreateViewModel
        {
            AnagraficaList = anagraficaList,
            TipoViolazioneList = tipoViolazioneList
        };

        return View(viewModel);
    }



    // POST: Verbale/Create, crea un nuovo verbale
    [HttpPost]
    public async Task<IActionResult> Create([Bind("Verbale")] VerbaleCreateViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _verbaleDAL.CreateVerbaleAsync(viewModel.Verbale);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Errore durante la creazione.");
            }
        }
        else
        {
           //Gestione in caso di errore, inutile attualmente
        }

        // Ricarica i dati per i dropdown
        var anagrafiche = await _anagraficaDAL.GetAnagraficheAsync();
        var tipoViolazioni = await _tipoViolazioneDAL.GetTipiViolazioneAsync();

        viewModel.AnagraficaList = new SelectList(anagrafiche.Select(a => new
        {
            a.Idanagrafica,
            DisplayText = $"{a.Nome} {a.Cognome}"
        }), "Idanagrafica", "DisplayText");

        viewModel.TipoViolazioneList = new SelectList(tipoViolazioni, "Idviolazione", "Descrizione");

        return View(viewModel);
    }




    private async Task<bool> VerbaleExists(int id)
    {
        var verbale = await _verbaleDAL.GetVerbaleByIdAsync(id);
        return verbale != null;
    }


    // GET: Verbale/Details/Id, restituisce i dettagli di un verbale
    public async Task<IActionResult> Details(int id)
    {
        var verbale = await _verbaleDAL.GetVerbaleByIdAsync(id);
        if (verbale == null)
        {
            return NotFound();
        }
        if (!verbale.Idanagrafica.HasValue || !verbale.Idviolazione.HasValue)
        {
            return NotFound(); 
        }

        var anagrafica = await _anagraficaDAL.GetAnagraficaByIdAsync(verbale.Idanagrafica.Value);
        if (anagrafica == null)
        {
            return NotFound();
        }

        var tipoViolazione = await _tipoViolazioneDAL.GetTipoViolazioneByIdAsync(verbale.Idviolazione.Value);
        if (tipoViolazione == null)
        {
            return NotFound();
        }

        var model = new VerbaleDetailsViewModel
        {
            Verbale = verbale,
            Anagrafica = anagrafica,
            TipoViolazione = tipoViolazione
        };

        return View(model);
    }
}

