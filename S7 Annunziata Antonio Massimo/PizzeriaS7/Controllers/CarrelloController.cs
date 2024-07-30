using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzeriaS7.Context;
using PizzeriaS7.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaS7.Controllers
{
    [Authorize]
    public class CarrelloController : Controller
    {
        private readonly PizzeriaContext _context;
        private readonly UserManager<Utente> _userManager;

        public CarrelloController(PizzeriaContext context, UserManager<Utente> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("api/Carrello/AddToCart")]
        public IActionResult AddToCart([FromBody] AddToCartRequest request)
        {
            var carrello = HttpContext.Session.GetString("Carrello");
            var carrelloItems = carrello == null ? new List<CarrelloItem>() : JsonConvert.DeserializeObject<List<CarrelloItem>>(carrello);

            var item = carrelloItems.FirstOrDefault(i => i.ProdottoId == request.ProdottoId);

            if (item == null)
            {
                carrelloItems.Add(new CarrelloItem { ProdottoId = request.ProdottoId, Quantity = request.Quantity });
            }
            else
            {
                item.Quantity += request.Quantity;
            }

            HttpContext.Session.SetString("Carrello", JsonConvert.SerializeObject(carrelloItems));

            return Ok(new { success = true, message = "Product added to cart" });
        }

        public IActionResult Index()
        {
            var carrello = HttpContext.Session.GetString("Carrello");
            var carrelloItems = carrello == null ? new List<CarrelloItem>() : JsonConvert.DeserializeObject<List<CarrelloItem>>(carrello);

            var prodotti = _context.Prodotti.ToList();
            var carrelloViewModel = from item in carrelloItems
                                    join prodotto in prodotti on item.ProdottoId equals prodotto.Id
                                    select new CarrelloViewModel
                                    {
                                        Prodotto = prodotto,
                                        Quantity = item.Quantity
                                    };

            return View(carrelloViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Ordine ordine)
        {
            var carrello = HttpContext.Session.GetString("Carrello");
            var carrelloItems = carrello == null ? new List<CarrelloItem>() : JsonConvert.DeserializeObject<List<CarrelloItem>>(carrello);

            if (!carrelloItems.Any())
            {
                ModelState.AddModelError("", "Il carrello è vuoto");
                return View("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            ordine.UtenteId = user.Id; // Assicurati che UtenteId sia una stringa
            ordine.DataOrdine = DateTime.Now;
            ordine.Evaso = false;

            _context.Ordini.Add(ordine);
            await _context.SaveChangesAsync();

            foreach (var item in carrelloItems)
            {
                var dettaglio = new DettaglioOrdine
                {
                    OrdineId = ordine.Id,
                    ProdottoId = item.ProdottoId,
                    Quantità = item.Quantity,
                    PrezzoTotale = _context.Prodotti.Find(item.ProdottoId).Prezzo * item.Quantity
                };
                _context.DettagliOrdine.Add(dettaglio);
            }

            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Carrello");

            return RedirectToAction("Confermato");
        }

        public IActionResult Confermato()
        {
            return View();
        }
    }
}
