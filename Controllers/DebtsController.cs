using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using radio_waves.Data;
using radio_waves.Models;

namespace radio_waves.Controllers
{
    public class DebtsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DebtsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.Debts.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Debt debt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(debt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(debt);
        }

        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var debt = await _context.Debts.FindAsync(id);
            if (debt == null) return NotFound();

            debt.IsPaid = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
