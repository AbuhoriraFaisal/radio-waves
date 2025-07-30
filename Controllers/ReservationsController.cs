
using Microsoft.AspNetCore.Mvc;
using radio_waves.Models;
using Microsoft.EntityFrameworkCore;
using radio_waves.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using radio_waves.Reports;
using QuestPDF.Fluent;
using Microsoft.AspNetCore.Authorization;

namespace radio_waves.Controllers
{

    [Authorize(Roles = "Admin,Receptionist")] 

    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Print(string? searchString, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Reservations
                .Include(r => r.Patient)
                .Include(r => r.RadiologyType)
                .Include(r => r.Technician)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(r => r.Patient.FullName.Contains(searchString) 
                || r.Technician.FullName.Contains(searchString));

            if (fromDate.HasValue)
                query = query.Where(r => r.AppointmentDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(r => r.AppointmentDate <= toDate.Value);

            var reservations = await query.ToListAsync();

            var report = new ReservationReport(reservations, searchString, fromDate, toDate);
            var pdf = report.GeneratePdf();

            return File(pdf, "application/pdf", "reservations.pdf");
        }

        public async Task<IActionResult> Index(string searchString, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Reservations
                .Include(r => r.Patient)
                .Include(r => r.RadiologyType)
                .Include(r => r.Technician)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(r => r.Patient.FullName.Contains(searchString) 
                                || r.Technician.FullName.Contains(searchString));
            }

            if (fromDate.HasValue)
            {
                query = query.Where(r => r.AppointmentDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(r => r.AppointmentDate <= toDate.Value);
            }

            var results = await query.ToListAsync();
            return View(results);
        }

        //public async Task<IActionResult> Index()
        //{
        //    var reservations = await _context.Reservations.Include(r => r.RadiologyType)
        //        .Include(r => r.Patient)
        //        .Include(r => r.Technician)
        //        .ToListAsync();
        //    return View(reservations);
        //}
        public async Task<IActionResult> Details(int id)
        {
            var reservations = await _context.Reservations.Include(r => r.RadiologyType)
                .Include(r => r.Patient)
                .Include(r => r.Shift)
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
                        d.TechnicianShare = 0;// d.Amount * (decimal)(shift.TechnicianPercentage / 100.0);
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
                        CompanyId = reservation.InsuranceId,
                        TechnicianShare = 0,//(decimal)((double)reservation.BasePrice * insurneCompany.CoveragedPercentage / 100.0) * (decimal)(shift.TechnicianPercentage / 100.0),
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
