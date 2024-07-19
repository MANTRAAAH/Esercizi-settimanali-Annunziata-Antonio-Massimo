using System.Data.SqlClient;
using GestioneContravvenzioni.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

public class TipoViolazioneController : Controller
{
    private readonly TipoViolazioneDAL _tipoViolazioneDAL;

    public TipoViolazioneController(TipoViolazioneDAL tipoViolazioneDAL)
    {
        _tipoViolazioneDAL = tipoViolazioneDAL;
    }

    // GET: TipoViolazione, restituisce una tupla di due liste di tipi di violazione, una per i tipi contestabili e una per i tipi non contestabili
    public async Task<IActionResult> Index()
    {
        var violazioni = await _tipoViolazioneDAL.GetTipiViolazioneAsync();
        var violazioniContestabili = violazioni.Where(v => v.IsContestabile).ToList();
        var violazioniNonContestabili = violazioni.Where(v => !v.IsContestabile).ToList();

        var model = new Tuple<IEnumerable<TipoViolazione>, IEnumerable<TipoViolazione>>(violazioniContestabili, violazioniNonContestabili);

        return View(model);
    }
}
