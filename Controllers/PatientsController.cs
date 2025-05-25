using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using radio_waves.Data;
using radio_waves.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace radio_waves.Controllers
{
    

    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.Patients.ToListAsync());

        public IActionResult Create()
        {
            ViewBag.Insurances = new SelectList(_context.Insurances, "Id", "Provider");
            return View();
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient Patient)
        {
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Or log it
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(Patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Patient);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Patient = await _context.Patients.FindAsync(id);
            ViewBag.Insurances = new SelectList(_context.Insurances, "Id", "Provider", Patient?.InsuranceId);
            return Patient == null ? NotFound() : View(Patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient Patient)
        {
            if (id != Patient.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(Patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Patient);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var Patient = await _context.Patients.FindAsync(id);
            if (Patient == null) return NotFound();

            _context.Patients.Remove(Patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
