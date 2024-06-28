using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScarpeCo.Models;
using ScarpeCo.Services;
using System;

namespace ScarpeCo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService; // Servizio per la gestione dei prodotti
        private readonly ILogger<ProductsController> _logger; // Logger per la registrazione di eventi

        // Costruttore per inizializzare il controller con il servizio e il logger necessari
        public ProductsController(ProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        // Azione per mostrare l'elenco dei prodotti
        public IActionResult Index()
        {
            var products = _productService.GetAllProducts(); // Ottiene tutti i prodotti dal servizio
            return View(products); // Visualizza la vista con l'elenco dei prodotti
        }

        // Azione per visualizzare i dettagli di un singolo prodotto
        public IActionResult Details(int id)
        {
            var product = _productService.GetProductById(id); // Ottiene il prodotto dal servizio tramite ID
            if (product == null)
            {
                return NotFound(); // Se il prodotto non esiste, restituisce 404 (Not Found)
            }
            return View(product); // Visualizza la vista con i dettagli del prodotto
        }

        // Azione per visualizzare il form di creazione di un nuovo prodotto
        public IActionResult Create()
        {
            return View();
        }

        // Azione HTTP POST per aggiungere un nuovo prodotto
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid) // Verifica che il modello sia valido
            {
                try
                {
                    _productService.AddProduct(product); // Aggiunge il prodotto tramite il servizio

                    TempData["Message"] = "Prodotto aggiunto con successo"; // Messaggio temporaneo per la conferma
                    return RedirectToAction(nameof(Index)); // Reindirizza alla pagina dell'elenco dei prodotti
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Si è verificato un errore durante il salvataggio del prodotto: {ex.Message}");
                    // Aggiunge un errore di validazione al modello nel caso ci siano problemi durante l'aggiunta del prodotto
                }
            }

            return View(product); // Se il modello non è valido, ritorna alla vista di creazione con i dati inseriti
        }
    }
}
