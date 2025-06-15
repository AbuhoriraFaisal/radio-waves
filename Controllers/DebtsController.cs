using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult Create()
        {
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "FullName");
            ViewBag.Reservations = new SelectList(_context.Reservations, "Id", "Id"); // Or any display value
            return View();
        }

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
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "FullName", debt.PatientId);
            ViewBag.Reservations = new SelectList(_context.Reservations, "Id", "Id", debt.ReservationId);
            return View(debt);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var debt = await _context.Debts.FindAsync(id);
            if (debt == null)
            {
                return NotFound();
            }
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "FullName", debt.PatientId);
            ViewBag.Reservations = new SelectList(_context.Reservations, "Id", "Id", debt.ReservationId);
            return View(debt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Debt debt)
        {
            if (id != debt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(debt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "FullName", debt.PatientId);
            ViewBag.Reservations = new SelectList(_context.Reservations, "Id", "Id", debt.ReservationId);
            return View(debt);
        }

        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var debt = await _context.Debts.FindAsync(id);
            if (debt == null) return NotFound();

            debt.IsPaid = !debt.IsPaid;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var debt = await _context.Debts.FindAsync(id);
            if (debt == null) return NotFound();

            _context.Debts.Remove(debt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
