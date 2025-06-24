using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using radio_waves.Data;
using radio_waves.Models;

namespace radio_waves.Controllers
{
    public class PartnersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartnersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.Partners.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partner settlement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(settlement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(settlement);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var settlement = await _context.Partners.FindAsync(id);
            if (settlement == null) return NotFound();

           // _context.PartnerSettlements.Remove(settlement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
