using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaS7.Context;

[Authorize]
public class OrdineController : Controller
{
    private readonly PizzeriaContext _context;

    public OrdineController(PizzeriaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Riepilogo()
    {
        // Logica per recuperare i prodotti nel carrello
        // ...

        return View(); // Passare i dati necessari alla vista
    }

    [HttpPost]
    public async Task<IActionResult> ConcludiOrdine(string indirizzo, string note)
    {
        // Logica per salvare l'ordine nel database
        // ...

        return RedirectToAction("Confermato");
    }

    public IActionResult Confermato()
    {
        return View();
    }
}
