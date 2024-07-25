using Microsoft.AspNetCore.Mvc;
using GestoreAlbergo.Models;
using GestoreAlbergo.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GestoreAlbergo.Controllers
{
    [Authorize(Roles = "Admin,Dipendente")]
    public class CamereController : Controller
    {
        private readonly ICameraService _cameraService;

        public CamereController(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        // GET: Camere
        public async Task<IActionResult> Index()
        {
            var camere = await _cameraService.GetAllCamerasAsync();
            return View(camere);
        }

        // GET: Camere/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var camera = await _cameraService.GetCameraByIdAsync(id);
            if (camera == null)
            {
                return NotFound();
            }
            return View(camera);
        }

        // GET: Camere/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Camere/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Numero,Descrizione,Tipologia")] Camera camera)
        {
            if (ModelState.IsValid)
            {
                await _cameraService.AddCameraAsync(camera);
                return RedirectToAction(nameof(Index));
            }
            return View(camera);
        }

        // GET: Camere/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var camera = await _cameraService.GetCameraByIdAsync(id);
            if (camera == null)
            {
                return NotFound();
            }
            return View(camera);
        }

        // POST: Camere/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero,Descrizione,Tipologia")] Camera camera)
        {
            if (id != camera.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _cameraService.UpdateCameraAsync(camera);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the camera.");
                }
            }
            return View(camera);
        }

        // GET: Camere/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var camera = await _cameraService.GetCameraByIdAsync(id);
            if (camera == null)
            {
                return NotFound();
            }
            return View(camera);
        }
    }
}
