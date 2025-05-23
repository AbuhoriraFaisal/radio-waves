using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using radio_waves.Data;
using radio_waves.Models;

namespace radio_waves.Controllers
{
    

    public class TechniciansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TechniciansController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Technicians.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Technician technician)
        {
            if (ModelState.IsValid)
            {
                _context.Add(technician);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(technician);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var technician = await _context.Technicians.FindAsync(id);
            return technician == null ? NotFound() : View(technician);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Technician technician)
        {
            if (id != technician.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(technician);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(technician);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var technician = await _context.Technicians.FindAsync(id);
            if (technician == null) return NotFound();

            _context.Technicians.Remove(technician);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
