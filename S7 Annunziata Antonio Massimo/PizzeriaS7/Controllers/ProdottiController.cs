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
    public class ProdottiController : Controller
    {
        private readonly PizzeriaContext _context;

        public ProdottiController(PizzeriaContext context)
        {
            _context = context;
        }

        // GET: Prodotti
        public async Task<IActionResult> Index()
        {
            var prodotti = await _context.Prodotti
                .Include(p => p.Ingredienti)
                .Include(p => p.Immagini) 
                .ToListAsync();

            return View(prodotti);
        }

        // GET: Prodotti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodotto = await _context.Prodotti
                .Include(p => p.Ingredienti) 
                .Include(p => p.Immagini) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prodotto == null)
            {
                return NotFound();
            }

            return View(prodotto);
        }



        private bool ProdottoExists(int id)
        {
            return _context.Prodotti.Any(e => e.Id == id);
        }
    }
}
