using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaS7.Context;
using PizzeriaS7.Models;
using System.Linq;
using System.Threading.Tasks;


namespace PizzeriaS7.Controllers
{
    [Authorize]
    public class OrdiniController : Controller
    {
        private readonly PizzeriaContext _context;

        public OrdiniController(PizzeriaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Riepilogo()
        {
            var utenteId = User.Identity.Name; // Assumendo che l'utente sia autenticato
            var ordine = await _context.Ordini
                .Where(o => o.Utente.UserName == utenteId && !o.Evaso)
                .Select(o => new OrdineViewModel
                {
                    OrdineId = o.Id,
                    DataOrdine = o.DataOrdine,
                    NomeUtente = o.Utente.UserName,
                    DettagliOrdine = o.DettagliOrdine.Select(d => new DettaglioOrdineViewModel
                    {
                        ProdottoNome = d.Prodotto.Nome,
                        Quantità = d.Quantità,
                        PrezzoTotale = d.PrezzoTotale
                    }).ToList(),
                    IndirizzoSpedizione = o.IndirizzoSpedizione,
                    Note = o.Note
                    // Non è necessario assegnare TotaleOrdine qui
                })
                .FirstOrDefaultAsync();

            if (ordine == null)
            {
                return NotFound();
            }

            // Non c'è bisogno di calcolare TotaleOrdine manualmente; è calcolato automaticamente quando accedi alla proprietà.

            return View(ordine);
        }


    }
}
