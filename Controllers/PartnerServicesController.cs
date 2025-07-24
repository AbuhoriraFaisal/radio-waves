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
    public class PartnerServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartnerServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PartnerServices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PartnerServices.Include(p => p.Partner).Include(p => p.Service);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PartnerServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerServices = await _context.PartnerServices
                .Include(p => p.Partner)
                .Include(p => p.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partnerServices == null)
            {
                return NotFound();
            }

            return View(partnerServices);
        }

        // GET: PartnerServices/Create
        public IActionResult Create()
        {
            ViewData["PartnerId"] = new SelectList(_context.Partners, "Id", "PartnerName");
            ViewData["ServiceId"] = new SelectList(_context.RadiologyTypes, "Id", "Name");
            return View();
        }

        // POST: PartnerServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PartnerId,ServiceId,Amount_Percentage")] PartnerServices partnerServices)
        {
            // Rule 1: Check if the partner already has an entry for this service
            bool alreadyExists = await _context.PartnerServices
                .AnyAsync(ps => ps.PartnerId == partnerServices.PartnerId && ps.ServiceId == partnerServices.ServiceId);

            if (alreadyExists)
            {
                ModelState.AddModelError("", "This partner is already assigned to this service.");
            }

            // Rule 2: Ensure total percentage does not exceed 100
            decimal totalPercentage = await _context.PartnerServices
                .Where(ps => ps.ServiceId == partnerServices.ServiceId)
                .SumAsync(ps => (decimal?)ps.Amount_Percentage) ?? 0;

            if (totalPercentage + partnerServices.Amount_Percentage > 100)
            {
                ModelState.AddModelError("", "Total amount percentage for this service cannot exceed 100%.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(partnerServices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ServiceId"] = new SelectList(_context.RadiologyTypes, "Id", "Name", partnerServices.ServiceId);
            ViewData["PartnerId"] = new SelectList(_context.Partners, "Id", "Name", partnerServices.PartnerId);
            return View(partnerServices);
        }


        // GET: PartnerServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerServices = await _context.PartnerServices.FindAsync(id);
            if (partnerServices == null)
            {
                return NotFound();
            }
            ViewData["PartnerId"] = new SelectList(_context.Partners, "Id", "PartnerName", partnerServices.PartnerId);
            ViewData["ServiceId"] = new SelectList(_context.RadiologyTypes, "Id", "Name", partnerServices.ServiceId);
            return View(partnerServices);
        }

        // POST: PartnerServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PartnerId,ServiceId,Amount_Percentage")] PartnerServices partnerServices)
        {
            if (id != partnerServices.Id)
            {
                return NotFound();
            }
            // Rule 2: Ensure total percentage does not exceed 100
            var partnerServicesList = await _context.PartnerServices.AsNoTracking().ToListAsync();

            decimal totalPercentage = partnerServicesList
                .Where(ps => ps.ServiceId == partnerServices.ServiceId)
                .Sum(ps => (decimal?)ps.Amount_Percentage) ?? 0;

            decimal oldPrecentage = partnerServicesList.
                Where(ps => ps.Id == partnerServices.Id).FirstOrDefault().Amount_Percentage;
                

            if (totalPercentage + partnerServices.Amount_Percentage - oldPrecentage > 100)
            {
                ModelState.AddModelError("", "Total amount percentage for this service cannot exceed 100%.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partnerServices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartnerServicesExists(partnerServices.Id))
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
            ViewData["PartnerId"] = new SelectList(_context.Partners, "Id", "PartnerName", partnerServices.PartnerId);
            ViewData["ServiceId"] = new SelectList(_context.RadiologyTypes, "Id", "Name", partnerServices.ServiceId);
            return View(partnerServices);
        }

        // GET: PartnerServices/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var partnerServices = await _context.PartnerServices
        //        .Include(p => p.Partner)
        //        .Include(p => p.Service)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (partnerServices == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(partnerServices);
        //}

        // POST: PartnerServices/Delete/5
       
        public async Task<IActionResult> Delete(int id)
        {
            var partnerServices = await _context.PartnerServices.FindAsync(id);
            if (partnerServices != null)
            {
                _context.PartnerServices.Remove(partnerServices);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartnerServicesExists(int id)
        {
            return _context.PartnerServices.Any(e => e.Id == id);
        }
    }
}
