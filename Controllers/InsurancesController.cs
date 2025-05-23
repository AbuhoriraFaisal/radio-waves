using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using radio_waves.Data;
using radio_waves.Models;

namespace radio_waves.Controllers
{
    public class InsurancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsurancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.Insurances.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insurance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insurance);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null) return NotFound();

            _context.Insurances.Remove(insurance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
