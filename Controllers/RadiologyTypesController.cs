using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using radio_waves.Data;
using radio_waves.Models;

namespace radio_waves.Controllers
{
   

    public class RadiologyTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RadiologyTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.RadiologyTypes.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RadiologyType radiologyType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(radiologyType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(radiologyType);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var radiologyType = await _context.RadiologyTypes.FindAsync(id);
            if (radiologyType == null)
            {
                return NotFound();
            }
            return View(radiologyType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RadiologyType radiologyType)
        {
            if (id != radiologyType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(radiologyType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(radiologyType);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var radiologyType = await _context.RadiologyTypes.FindAsync(id);
            if (radiologyType == null)
            {
                return NotFound();
            }

            _context.RadiologyTypes.Remove(radiologyType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
