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

        public async Task<IActionResult> Index()
        {
            var prodotti = await _context.Prodotti.ToListAsync();
            return View(prodotti);
        }
    }
}
