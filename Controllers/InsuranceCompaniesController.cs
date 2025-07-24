using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using radio_waves.Data;
using radio_waves.Models;

namespace radio_waves.Controllers
{
    [Authorize(Roles = "Admin,Receptionist")]
    public class InsuranceCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsuranceCompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InsuranceCompanies
        public async Task<IActionResult> Index()
        {
            return View(await _context.InsuranceCompanies.ToListAsync());
        }

        // GET: InsuranceCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceCompanies = await _context.InsuranceCompanies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceCompanies == null)
            {
                return NotFound();
            }

            return View(insuranceCompanies);
        }

        // GET: InsuranceCompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InsuranceCompanies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Provider,PolicyNumber,CoverageDetails,CoveragedPercentage")] InsuranceCompanies insuranceCompanies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insuranceCompanies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insuranceCompanies);
        }

        // GET: InsuranceCompanies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceCompanies = await _context.InsuranceCompanies.FindAsync(id);
            if (insuranceCompanies == null)
            {
                return NotFound();
            }
            return View(insuranceCompanies);
        }

        // POST: InsuranceCompanies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Provider,PolicyNumber,CoverageDetails,CoveragedPercentage")] InsuranceCompanies insuranceCompanies)
        {
            if (id != insuranceCompanies.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuranceCompanies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceCompaniesExists(insuranceCompanies.Id))
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
            return View(insuranceCompanies);
        }

        // GET: InsuranceCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceCompanies = await _context.InsuranceCompanies.FindAsync(id);
            if (insuranceCompanies == null)
            {
                return NotFound();
            }
            _context.InsuranceCompanies.Remove(insuranceCompanies);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: InsuranceCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insuranceCompanies = await _context.InsuranceCompanies.FindAsync(id);
            if (insuranceCompanies != null)
            {
                _context.InsuranceCompanies.Remove(insuranceCompanies);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceCompaniesExists(int id)
        {
            return _context.InsuranceCompanies.Any(e => e.Id == id);
        }
    }
}
