using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using radio_waves.Data;
using radio_waves.Models;

namespace radio_waves.Controllers
{
    

    public class ShiftsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShiftsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.Shifts.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Shift shift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shift);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            return shift == null ? NotFound() : View(shift);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Shift shift)
        {
            if (id != shift.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shift);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null) return NotFound();

            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
