using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using radio_waves.Data;
using radio_waves.Models;

namespace radio_waves.Controllers
{
    public class ExpendituresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpendituresController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.Expenditures.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expenditure expenditure)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expenditure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenditure);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var expenditure = await _context.Expenditures.FindAsync(id);
            if (expenditure == null)
            {
                return NotFound();
            }
            return View(expenditure);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Expenditure expenditure)
        {
            if (id != expenditure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(expenditure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenditure);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var expenditure = await _context.Expenditures.FindAsync(id);
            if (expenditure == null) return NotFound();

            _context.Expenditures.Remove(expenditure);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
