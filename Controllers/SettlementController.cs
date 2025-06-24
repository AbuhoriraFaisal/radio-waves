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
            var targetDate = DateTime.Today;
            var technicians = await _context.Technicians.ToListAsync();
            var shifts = await _context.Shifts.ToListAsync();
            var viewModels = new List<TechnicianSettlementViewModel>();

            foreach (var tech in technicians)
            {

                var reservations = await _context.Reservations
                    .Where(r => r.TechnicianId == tech.Id)
                    .ToListAsync(); // Pull data into memory


                if (!reservations.Any()) continue;

                var debts = await _context.Debts
                    .Where(d => d.TechnicianId == tech.Id && d.IsPaid && !d.IsTechnicianShared && !d.IsSealed)
                    .ToListAsync();



                var insurances = await _context.Insurances
                    .Where(i => i.TechnicianId == tech.Id && i.IsComplete && !i.IsTechnicianShared && !i.IsSealed)
                    .ToListAsync();

                var totalReservations = reservations.Sum(r => r.TechnicianShare);
                var totalInsurance = insurances.Sum(i => i.TechnicianShare);
                var totalDebt = debts.Sum(d => d.TechnicianShare);
                //var totalExpenses = expenditures.Sum(e => e.Amount);

                var viewModel = new TechnicianSettlementViewModel
                {
                    TechnicianName = tech.FullName,
                    TotalFromReservations = Math.Round(totalReservations, 2),
                    TotalFromInsurance = Math.Round(totalInsurance, 2),
                    TotalDebtShare = Math.Round(totalDebt, 2),
                    //TotalExpenses = Math.Round(totalExpenses, 2),
                    SettelmentDate = targetDate,
                    NetPayable = Math.Round(totalReservations + totalInsurance + totalDebt, 2)
                };

                viewModels.Add(viewModel);

            }

            ViewBag.SummaryDate = targetDate.ToShortDateString();
            return View("TechnicianSummary", viewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TechniciansSettlement(List<TechnicianSettlement> Settlements)
        {
            if (Settlements != null && Settlements.Sum(i => i.TotalFromReservations) == 0 && Settlements.Sum(i => i.NetPayable) == 0)
                return View(await _context.PartnerSettlements.ToListAsync());
            await _context.Reservations
            .Where(r => !r.IsSealed)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsSealed, true));

            await _context.Insurances
            .Where(r => r.IsComplete && r.IsTechnicianShared && !r.IsSealed)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsSealed, true));

            await _context.Debts
            .Where(r => r.IsTechnicianShared && r.IsPaid && !r.IsSealed)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsSealed, true));

            await _context.Expenditures
            //.Where(r => !r.IsSealed)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsSealed, true));

            await _context.TechnicianSettlements.AddRangeAsync(Settlements);
            await _context.SaveChangesAsync();
            Settlements = null;
            return View(await _context.TechnicianSettlements.ToListAsync());

        }

        [HttpGet]

        public async Task<IActionResult> TechniciansSettlement()
        {

            return View(await _context.TechnicianSettlements.ToListAsync());
        }

        /*------------------------------------------------ Insurance Companies---------------------------------------*/
        public async Task<IActionResult> InsuranceCompanySummary()
        {
            var targetDate = DateTime.Today;
            var companies = await _context.InsuranceCompanies.ToListAsync();
            var viewModels = new List<InsuranceCompanySettlement>();

            foreach (var company in companies)
            {
                var insurances = await _context.Insurances
                    .Where(i => i.ProviderId == company.Id && !i.IsComplete && !i.IsSealed)
                    .ToListAsync();
                if (!insurances.Any()) continue;

                var totalInsuranceAmount = insurances.Sum(i => i.InsuranceAmount);

                viewModels.Add(new InsuranceCompanySettlement
                {
                    InsuranceCompanyName = company.Provider,
                    TotalInsuranceShare = Math.Round(totalInsuranceAmount, 2),
                    NetPayable = Math.Round(totalInsuranceAmount, 2),
                    SettlementDate = targetDate
                });
            }

            ViewBag.SummaryDate = targetDate.ToShortDateString();
            return View("InsuranceCompanySummary", viewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsuranceCompanySettlement(List<InsuranceCompanySettlement> Settlements)
        {
            if ( Settlements.Sum(i => i.TotalInsuranceShare) == 0)
                return View(await _context.PartnerSettlements.ToListAsync());
            

            await _context.Insurances
            .Where(r => r.IsComplete && !r.IsTechnicianShared && !r.IsSealed)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsComplete, true));

            await _context.InsuranceCompanySettlements.AddRangeAsync(Settlements);
            await _context.SaveChangesAsync();
            Settlements = null;
            return View(await _context.InsuranceCompanySettlements.ToListAsync());

        }

        [HttpGet]

        public async Task<IActionResult> InsuranceCompanySettlement()
        {

            return View(await _context.InsuranceCompanySettlements.ToListAsync());
        }

        /*------------------------------------------- Partner Part --------------------------------------------------------*/

        public async Task<IActionResult> PartnerSummary()
        {

            var targetDate = DateTime.Today;
            var partners = await _context.Partners.ToListAsync();
            var summaries = new List<PartnerSettlement>();
            var reservationsPaiedamount = await _context.Reservations
               .Where(r => !r.IsSealed)
               .SumAsync(r => r.PaiedAmount);

            var reservationsTchentionShareAmount = await _context.Reservations
               .Where(r => !r.IsSealed)
               .SumAsync(r => r.TechnicianShare);

            var insurancesPaiedamount = await _context.Insurances
                .Where(r => r.IsComplete && r.IsTechnicianShared && !r.IsSealed)
                .SumAsync(r => r.InsuranceAmount);

            var insurancesTchentionShareAmount = await _context.Insurances
               .Where(r => r.IsTechnicianShared && !r.IsSealed)
               .SumAsync(r => r.TechnicianShare);

            var debtsTchentionShareAmount = await _context.Debts
               .Where(r => r.IsTechnicianShared && !r.IsSealed)
               .SumAsync(r => r.TechnicianShare);

            var debtspaiedAmount = await _context.Debts
               .Where(r => r.IsPaid && r.IsTechnicianShared && !r.IsSealed)
               .SumAsync(r => r.Amount);


            var expenses = await _context.Expenditures
                .Where(r => !r.IsSealed)
                .SumAsync(r => r.Amount);
            //var partner = await _context.Partners;


            foreach (var partner in partners)
            {
                var partnerPercentage = partner.Amount_Percentage;

                var distributable = (reservationsPaiedamount + insurancesPaiedamount + debtspaiedAmount) - (reservationsTchentionShareAmount - insurancesTchentionShareAmount - debtsTchentionShareAmount - expenses);
                var partnerShare = distributable * (partnerPercentage / 100);

                var partenersettment = new PartnerSettlement
                {
                    PartnerName = partner.PartnerName,
                    Amount = Math.Round(partnerShare, 2),
                    SettlementDate = targetDate
                };

                summaries.Add(partenersettment);
            }

            ViewBag.SummaryDate = targetDate.ToShortDateString();
            return View("PartnerSummary", summaries);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartnersSettlement(List<PartnerSettlement> Settlements)
        {
            if (Settlements != null && Settlements.Sum(i => i.Amount) == 0)
                return View(await _context.PartnerSettlements.ToListAsync());
            await _context.Reservations
            .Where(r => !r.IsSealed)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsSealed, true));

            await _context.Insurances
            .Where(r => r.IsComplete && r.IsTechnicianShared  && !r.IsSealed)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsSealed, true));

            await _context.Debts
            .Where(r => r.IsTechnicianShared && r.IsPaid && !r.IsSealed)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsSealed, true));

            await _context.Expenditures
            //.Where(r => !r.IsSealed)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsSealed, true));

            await _context.PartnerSettlements.AddRangeAsync(Settlements);
            await _context.SaveChangesAsync();
            Settlements = null;
            return View(await _context.PartnerSettlements.ToListAsync());

        }

        [HttpGet]
        
        public async Task<IActionResult> PartnersSettlement()
        {
            
                return View(await _context.PartnerSettlements.ToListAsync());
        }
    }
}