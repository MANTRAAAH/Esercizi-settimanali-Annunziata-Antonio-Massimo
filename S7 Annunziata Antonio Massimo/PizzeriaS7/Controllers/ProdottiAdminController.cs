using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaS7.Context;
using PizzeriaS7.Models;
using System.Threading.Tasks;

namespace PizzeriaS7.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProdottiAdminController : Controller
    {
        private readonly PizzeriaContext _context;

        public ProdottiAdminController(PizzeriaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Prodotti.ToListAsync());
        }

        // Aggiungi azioni per Create, Edit, Delete
    }
}
