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
    [Authorize(Roles = "Admin")]
    public class ExpendituresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpendituresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Expenditures
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Expenditures.Include(e => e.RadiologyType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Expenditures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenditure = await _context.Expenditures
                .Include(e => e.RadiologyType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenditure == null)
            {
                return NotFound();
            }

            return View(expenditure);
        }

        // GET: Expenditures/Create
        public IActionResult Create()
        {
            var types = _context.RadiologyTypes
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                })
                .ToList();

            // Add default/optional option
            types.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- Select Radiology Type --"
            });

            ViewData["RadiologyTypeId"] = types;

            return View();
        }


        // POST: Expenditures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Category,Amount,IsSealed,RadiologyTypeId")] Expenditure expenditure)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expenditure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RadiologyTypeId"] = new SelectList(_context.RadiologyTypes, "Id", "Id", expenditure.RadiologyTypeId);
            return View(expenditure);
        }

        // GET: Expenditures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenditure = await _context.Expenditures.FindAsync(id);
            if (expenditure == null)
            {
                return NotFound();
            }
            ViewData["RadiologyTypeId"] = new SelectList(_context.RadiologyTypes, "Id", "Name", expenditure.RadiologyTypeId);
            return View(expenditure);
        }

        // POST: Expenditures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Category,Amount,IsSealed,RadiologyTypeId")] Expenditure expenditure)
        {
            if (id != expenditure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenditure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenditureExists(expenditure.Id))
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
            ViewData["RadiologyTypeId"] = new SelectList(_context.RadiologyTypes, "Id", "Id", expenditure.RadiologyTypeId);
            return View(expenditure);
        }

        // GET: Expenditures/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var expenditure = await _context.Expenditures
        //        .Include(e => e.RadiologyType)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (expenditure == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(expenditure);
        //}

        // POST: Expenditures/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var expenditure = await _context.Expenditures.FindAsync(id);
            if (expenditure != null)
            {
                _context.Expenditures.Remove(expenditure);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenditureExists(int id)
        {
            return _context.Expenditures.Any(e => e.Id == id);
        }
    }
}
