using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ILogger<CarrelloController> _logger; // Inietta il logger

        public CarrelloController(PizzeriaContext context, UserManager<Utente> userManager, ILogger<CarrelloController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger; // Assegna il logger
        }


        public IActionResult Index()
        {
            var carrello = HttpContext.Session.GetString("Carrello");
            var carrelloItems = carrello == null ? new List<CarrelloItem>() : JsonConvert.DeserializeObject<List<CarrelloItem>>(carrello);

            var prodotti = _context.Prodotti.Include(p => p.Ingredienti).ToList();
            var carrelloViewModel = from item in carrelloItems
                                    join prodotto in prodotti on item.ProdottoId equals prodotto.Id
                                    select new CarrelloViewModel
                                    {
                                        Prodotto = prodotto,
                                        Quantity = item.Quantity,
                                        PrezzoTotale = prodotto.Prezzo * item.Quantity
                                    };

            ViewBag.AllIngredienti = _context.Ingredienti.ToList();
            return View(carrelloViewModel);
        }

        public IActionResult Checkout()
        {
            var carrello = HttpContext.Session.GetString("Carrello");
            var carrelloItems = carrello == null ? new List<CarrelloItem>() : JsonConvert.DeserializeObject<List<CarrelloItem>>(carrello);

            if (!carrelloItems.Any())
            {
                ModelState.AddModelError("", "Il carrello è vuoto");
                return RedirectToAction("Index");
            }

            var prodottiConDettagli = new List<(Prodotto prodotto, int quantity)>();

            foreach (var item in carrelloItems)
            {
                var prodotto = _context.Prodotti.FirstOrDefault(p => p.Id == item.ProdottoId);
                if (prodotto != null)
                {
                    prodottiConDettagli.Add((prodotto, item.Quantity));
                }
            }

            var checkoutViewModel = new CheckoutViewModel
            {
                Ordine = new Ordine(),
                CarrelloItems = prodottiConDettagli
            };

            return View(checkoutViewModel);
        }





[HttpPost]
public async Task<IActionResult> Checkout(CheckoutViewModel model)
{
    var carrello = HttpContext.Session.GetString("Carrello");
    var carrelloItems = carrello == null ? new List<CarrelloItem>() : JsonConvert.DeserializeObject<List<CarrelloItem>>(carrello);

    if (!carrelloItems.Any())
    {
        ModelState.AddModelError("", "Il carrello è vuoto");
        return View(model);
    }

    var user = await _userManager.GetUserAsync(User);
    model.Ordine.UtenteId = user.Id;
    model.Ordine.DataOrdine = DateTime.Now;
    model.Ordine.Evaso = false;

    _context.Ordini.Add(model.Ordine);
    await _context.SaveChangesAsync();

    foreach (var item in carrelloItems)
    {
        var prodotto = await _context.Prodotti.Include(p => p.Ingredienti).FirstOrDefaultAsync(p => p.Id == item.ProdottoId);
        var prezzoIngredientiExtra = item.IngredientiAggiuntiIds.Count * 1.50m;
        var prezzoTotaleProdotto = (prodotto.Prezzo + prezzoIngredientiExtra) * item.Quantity;

        var dettaglio = new DettaglioOrdine
        {
            OrdineId = model.Ordine.Id,
            ProdottoId = item.ProdottoId,
            Quantità = item.Quantity,
            PrezzoTotale = prezzoTotaleProdotto
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



        [HttpPost]
        [Route("api/Carrello/AddToCart")]
        public IActionResult AddToCart([FromBody] AddToCartRequest request)
        {
            var carrello = HttpContext.Session.GetString("Carrello");
            var carrelloItems = carrello == null ? new List<CarrelloItem>() : JsonConvert.DeserializeObject<List<CarrelloItem>>(carrello);

            var prodotto = _context.Prodotti.Include(p => p.Ingredienti).FirstOrDefault(p => p.Id == request.ProdottoId);

            if (prodotto == null)
            {
                return NotFound(new { success = false, message = "Prodotto non trovato" });
            }

            var item = carrelloItems.FirstOrDefault(i => i.ProdottoId == request.ProdottoId);

            if (item == null)
            {
                item = new CarrelloItem
                {
                    ProdottoId = request.ProdottoId,
                    Quantity = request.Quantity,
                    IngredientiBaseIds = prodotto.Ingredienti.Select(i => i.Id).ToList(),
                    IngredientiAggiuntiIds = new List<int>()
                };

                carrelloItems.Add(item);
            }
            else
            {
                item.Quantity += request.Quantity;
            }

            HttpContext.Session.SetString("Carrello", JsonConvert.SerializeObject(carrelloItems));

            return Ok(new { success = true, message = "Prodotto aggiunto al carrello" });
        }

        [HttpPost]
        [Route("Carrello/UpdateIngredienti")]
        public IActionResult UpdateIngredienti(UpdateIngredientiRequest request)
        {
            var carrello = HttpContext.Session.GetString("Carrello");
            var carrelloItems = carrello == null ? new List<CarrelloItem>() : JsonConvert.DeserializeObject<List<CarrelloItem>>(carrello);

            var item = carrelloItems.FirstOrDefault(i => i.ProdottoId == request.ProdottoId);

            if (item != null)
            {
                if (request.Aggiunto)
                {
                    if (!item.IngredientiAggiuntiIds.Contains(request.IngredienteId) && !item.IngredientiBaseIds.Contains(request.IngredienteId))
                    {
                        item.IngredientiAggiuntiIds.Add(request.IngredienteId);
                    }
                }
                else
                {
                    item.IngredientiAggiuntiIds.Remove(request.IngredienteId);
                }

                HttpContext.Session.SetString("Carrello", JsonConvert.SerializeObject(carrelloItems));

                TempData["SuccessMessage"] = "Ingredienti aggiornati con successo!";
            }
            else
            {
                TempData["ErrorMessage"] = "Prodotto non trovato nel carrello.";
            }

            return RedirectToAction("Index");
        }






    }
}
