using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using radio_waves.Data;
using radio_waves.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace radio_waves.Controllers
{
    public class SettlementController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SettlementController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> TechnicianSummary()
        {
            var targetDate =  DateTime.Today;
            var technicians = await _context.Technicians.ToListAsync();
            var shifts = await _context.Shifts.ToListAsync();
            var viewModels = new List<TechnicianSettlementViewModel>();

            foreach (var tech in technicians)
            {
                foreach (var shift in shifts)
                {
                    var reservations = await _context.Reservations
                        .Where(r => r.TechnicianId == tech.Id && r.ShiftId == shift.Id)
                        .ToListAsync(); // Pull data into memory

                    reservations = reservations
                        .Where(r => r.ReservationDate.Date == targetDate.Date)
                        .ToList(); // Apply .Date safely in memory



                    if (!reservations.Any()) continue;

                    var debts = await _context.Debts
                        .Where(d => d.TechnicianId == tech.Id)
                        .ToListAsync();

                    debts = debts
                        .Where(d => d.DebtDate.Date == targetDate.Date)
                        .ToList();

                    var insurances = await _context.Insurances
                        .Where(i => i.TechnicianId == tech.Id)
                        .ToListAsync();

                    insurances = insurances
                        .Where(i => i.InsuranceDate.Date == targetDate.Date)
                        .ToList();


                    //var expenditures = await _context.Expenditures
                    //    .Where(e => e.TechnicianId == tech.Id && e.Date.Date == targetDate)
                    //    .ToListAsync();

                    var totalReservations = reservations.Sum(r => r.TechnicianShare);
                    var totalInsurance = insurances.Sum(i => i.TechnicianShare);
                    var totalDebt = debts.Sum(d => d.TechnicianShare);
                    //var totalExpenses = expenditures.Sum(e => e.Amount);

                    var viewModel = new TechnicianSettlementViewModel
                    {
                        TechnicianName = $"{tech.FullName} - {shift.Name}",
                        TotalFromReservations = Math.Round(totalReservations, 2),
                        TotalFromInsurance = Math.Round(totalInsurance, 2),
                        TotalDebtShare = Math.Round(totalDebt, 2),
                        //TotalExpenses = Math.Round(totalExpenses, 2),
                        SettelmentDate = targetDate,
                        NetPayable = Math.Round(totalReservations + totalInsurance + totalDebt, 2)
                    };

                    viewModels.Add(viewModel);
                }
            }

            ViewBag.SummaryDate = targetDate.ToShortDateString();
            return View("TechnicianSummary", viewModels);
        }
    }


}
