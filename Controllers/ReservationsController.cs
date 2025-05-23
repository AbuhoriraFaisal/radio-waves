using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using radio_waves.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using radio_waves.Data;

namespace radio_waves.Controllers
{
    

    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations.Include(r => r.RadiologyType).ToListAsync();
            return View(reservations);
        }

        public IActionResult Create()
        {
            ViewBag.RadiologyTypes = _context.RadiologyTypes.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.RadiologyTypes = _context.RadiologyTypes.ToList();
            return View(reservation);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
