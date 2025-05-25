using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using radio_waves.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using radio_waves.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var now = DateTime.Now.TimeOfDay;
            var currentShift = _context.Shifts.FirstOrDefault(s => now >= s.StartTime && now <= s.EndTime);

            ViewBag.Patients = new SelectList(_context.Patients, "Id", "FullName");
            ViewBag.Technicians = new SelectList(_context.Technicians, "Id", "FullName");
            ViewBag.RadiologyTypesSelectList = new SelectList(_context.RadiologyTypes, "Id", "Name");
            ViewBag.ShiftsSelectList = new SelectList(_context.Shifts, "Id", "Name", currentShift?.Id);
            ViewBag.Insurances = new SelectList(_context.Insurances, "Id", "Provider");
            
            ViewBag.InsurancesJson = _context.Insurances
                .Select(i => new { i.Id, i.CoveragedPercentage })
                .ToList();

            ViewBag.RadiologyTypesJson = _context.RadiologyTypes.
                                            Select(rt => new { rt.Id, rt.Price }
                                            ).ToList();

            ViewBag.ShiftsJson = _context.Shifts
                .Select(s => new { s.Id, s.TechnicianPercentage })
                .ToList();

            return View(new Reservation { ShiftId = currentShift?.Id ?? 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            var radiologyType = await _context.RadiologyTypes.FindAsync(reservation.RadiologyTypeId);
            var shift = await _context.Shifts.FindAsync(reservation.ShiftId);

            if (radiologyType == null || shift == null)
            {
                ModelState.AddModelError("", "Invalid Radiology Type or Shift selected.");
            }
            if (ModelState.IsValid)
            {
                // Set base price from RadiologyType
                reservation.BasePrice = radiologyType.Price;

                // Calculate Technician Share
                reservation.TechnicianShare = reservation.TotalPrice * (decimal)(shift.TechnicianPercentage / 100.0);

                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "FullName", reservation.PatientId);
            ViewBag.RadiologyTypes = new SelectList(_context.RadiologyTypes, "Id", "Name", reservation.RadiologyTypeId);
            ViewBag.Technicians = new SelectList(_context.Technicians, "Id", "FullName", reservation.TechnicianId);
            ViewBag.Shifts = new SelectList(_context.Shifts, "Id", "Name", reservation.ShiftId);

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
