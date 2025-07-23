using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using radio_waves.Data;
using radio_waves.Models;
using radio_waves.Reports;

namespace radio_waves.Controllers
{
    public class InsurancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsurancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Insurances
        public async Task<IActionResult> Index(string? filter)
        {
            var query = _context.Insurances.Include(i => i.Patient).Include(i=>i.Company).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(i =>
                    (i.Patient != null && i.Patient.FullName.Contains(filter)) ||
                    (i.Company.Provider != null && i.Company.Provider.Contains(filter)));
            }

            var result = await query.ToListAsync();
            return View(result);
        }


        // GET: Insurances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // GET: Insurances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insurances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProviderId,PatientId,ReservationId,TechnicianId,TechnicianShare,PaidAmount,InsuranceAmount,PolicyNumber,IsComplete")] Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insurance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insurance);
        }

        // GET: Insurances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }
            return View(insurance);
        }

        // POST: Insurances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProviderId,PatientId,ReservationId,TechnicianId,TechnicianShare,PaidAmount,InsuranceAmount,PolicyNumber,IsComplete")] Insurance insurance)
        {
            if (id != insurance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insurance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceExists(insurance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(insurance);
        }

        public async Task<IActionResult> MarkAsComplete(int id)
        {
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null) return NotFound();

            insurance.IsComplete = !insurance.IsComplete;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> MarkAsTechnicianShared(int id)
        {
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null) return NotFound();

            insurance.IsTechnicianShared = !insurance.IsTechnicianShared;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Insurances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // POST: Insurances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance != null)
            {
                _context.Insurances.Remove(insurance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceExists(int id)
        {
            return _context.Insurances.Any(e => e.Id == id);
        }

        public IActionResult DownloadInsuranceReport(string? filter)
        {
            var query = _context.Insurances.Include(i => i.Patient).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(i =>
                    (i.Patient != null && i.Patient.FullName.Contains(filter)) ||
                    (i.Company.Provider != null && i.Company.Provider.Contains(filter)));
            }

            var list = query.ToList();
            var report = new InsuranceReport(list, filter);

            var stream = new MemoryStream();
            report.GeneratePdf(stream);
            stream.Position = 0;

            return File(stream, "application/pdf", "InsuranceReport.pdf");
        }

    }
}
