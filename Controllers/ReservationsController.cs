using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using radio_waves.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using radio_waves.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Versioning;

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
            var reservations = await _context.Reservations.Include(r => r.RadiologyType)
                .Include(r => r.Patient)
                .Include(r => r.Technician)
                .ToListAsync();
            return View(reservations);
        }
        public async Task<IActionResult> Details(int id)
        {
            var reservations = await _context.Reservations.Include(r => r.RadiologyType)
                .Include(r => r.Patient)
                .Include(r => r.Technician).FirstOrDefaultAsync(r => r.Id==id); // Assuming you want to show details of a specific reservation
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
            ViewBag.InsuranceCompanies = new SelectList(_context.InsuranceCompanies, "Id", "Provider");

            ViewBag.InsuranceCompaniesJson = _context.InsuranceCompanies
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
            var insurneCompany = await _context.InsuranceCompanies.FindAsync(reservation.InsuranceId);


            if (radiologyType == null || shift == null)
            {
                ModelState.AddModelError("", "Invalid Radiology Type or Shift selected.");
            }
            if (ModelState.IsValid)
            {
                // Set base price from RadiologyType
                reservation.BasePrice = radiologyType.Price;
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                // Calculate Technician Share
                // reservation.TechnicianShare = reservation.PaiedAmount * (decimal)(shift.TechnicianPercentage / 100.0);
                if (reservation.IsDebt)
                {
                    Debt d = new Debt()
                    {
                        PatientId = reservation.PatientId,
                        ReservationId = reservation.Id,
                        IsPaid = false,
                        Amount = reservation.BasePrice - reservation.PaiedAmount,
                        DueDate = DateTime.Now,
                        Comments = "test",
                        TechnicianShare = (reservation.BasePrice - reservation.PaiedAmount) * (decimal)(shift.TechnicianPercentage / 100.0),
                        TechnicianId = reservation.TechnicianId
                    };
                    if (reservation.CoveredByInsurance)
                    {
                        d.Amount = d.Amount - (decimal)((double)reservation.BasePrice * insurneCompany.CoveragedPercentage / 100.0);
                        d.TechnicianShare = d.Amount * (decimal)(shift.TechnicianPercentage / 100.0);
                    }
                    _context.Add(d);
                    await _context.SaveChangesAsync();
                }

                if (reservation.CoveredByInsurance)
                {
                    Insurance I = new Insurance()
                    {
                        PatientId = reservation.PatientId,
                        ReservationId = reservation.Id,
                        IsComplete = false,
                        InsuranceAmount = (decimal)((double)reservation.BasePrice * insurneCompany.CoveragedPercentage / 100.0),
                        PaidAmount = reservation.PaiedAmount,
                        PolicyNumber = "1234567",
                        ProviderId = reservation.InsuranceId,
                        TechnicianShare = (decimal)((double)reservation.BasePrice * insurneCompany.CoveragedPercentage / 100.0) * (decimal)(shift.TechnicianPercentage / 100.0),
                        TechnicianId = reservation.TechnicianId
                    };
                    _context.Add(I);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "FullName", reservation.PatientId);
            ViewBag.RadiologyTypes = new SelectList(_context.RadiologyTypes, "Id", "Name", reservation.RadiologyTypeId);
            ViewBag.Technicians = new SelectList(_context.Technicians, "Id", "FullName", reservation.TechnicianId);
            ViewBag.Shifts = new SelectList(_context.Shifts, "Id", "Name", reservation.ShiftId);

            return View(reservation);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            var radiologyType = await _context.RadiologyTypes.FindAsync(reservation.RadiologyTypeId);
            var shift = await _context.Shifts.FindAsync(reservation.ShiftId);
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "FullName", reservation.PatientId);
            ViewBag.RadiologyTypes = new SelectList(_context.RadiologyTypes, "Id", "Name", reservation.RadiologyTypeId);
            ViewBag.Technicians = new SelectList(_context.Technicians, "Id", "FullName", reservation.TechnicianId);
            ViewBag.Shifts = new SelectList(_context.Shifts, "Id", "Name", reservation.ShiftId);

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var radiologyType = await _context.RadiologyTypes.FindAsync(reservation.RadiologyTypeId);
            var shift = await _context.Shifts.FindAsync(reservation.ShiftId);
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
            reservation.IsCanceled = !reservation.IsCanceled; // Mark as canceled instead of deleting
            _context.Reservations.Update(reservation);
            // 1. Update Insurance
            await _context.Insurances
            .Where(r => r.ReservationId==id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsCanceled, reservation.IsCanceled));
            //Update Debts
            await _context.Debts
            .Where(r => r.ReservationId==id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsCanceled, reservation.IsCanceled));
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
