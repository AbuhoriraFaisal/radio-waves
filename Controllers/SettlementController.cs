using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using radio_waves.Data;
using radio_waves.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace radio_waves.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettlementController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SettlementController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> TechnicianSummary()
        {
            var targetDate = DateTime.Today;
            var technicians = await _context.Technicians.ToListAsync();
            var shifts = await _context.Shifts.ToListAsync();
            var viewModels = new List<TechnicianSettlement>();

            foreach (var tech in technicians)
            {

                var reservations = await _context.Reservations
                    .Where(r => r.TechnicianId == tech.Id && !r.IsTechnicianShared && !r.IsCanceled)
                    .ToListAsync(); // Pull data into memory
                //try to check the git hub


                if (!reservations.Any()) continue;

                var debts = await _context.Debts
                    .Where(d => d.TechnicianId == tech.Id && d.IsPaid && !d.IsTechnicianShared && !d.IsSealed && !d.IsCanceled)
                    .ToListAsync();



                var insurances = await _context.Insurances
                    .Where(i => i.TechnicianId == tech.Id && i.IsComplete && !i.IsTechnicianShared && !i.IsSealed && !i.IsCanceled)
                    .ToListAsync();

                var totalReservations = reservations.Sum(r => r.TechnicianShare);
                var totalInsurance = insurances.Sum(i => i.TechnicianShare);
                var totalDebt = debts.Sum(d => d.TechnicianShare);
                //var totalExpenses = expenditures.Sum(e => e.Amount);

                var viewModel = new TechnicianSettlement
                {
                    TechnicianId = tech.Id,
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
            if (Settlements == null || Settlements.Sum(i => i.NetPayable) == 0)
                return RedirectToAction(nameof(TechniciansSettlement));

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Update Insurance
                await _context.Insurances
            .Where(r => r.IsComplete && !r.IsTechnicianShared && !r.IsSealed && !r.IsCanceled)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsTechnicianShared, true));
                //Update Debts
                await _context.Debts
                .Where(r => !r.IsTechnicianShared && r.IsPaid && !r.IsSealed && !r.IsCanceled)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(r => r.IsTechnicianShared, true));

                //Updtae Reservation
                await _context.Reservations.Where(r => !r.IsTechnicianShared && !r.IsSealed && !r.IsCanceled)
                                           .ExecuteUpdateAsync(s => s.SetProperty(r => r.IsTechnicianShared,
                                           true));

                await _context.TechnicianSettlements.AddRangeAsync(Settlements);
                await _context.SaveChangesAsync();

                // 4. Commit all if successful
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                // 5. Rollback on error
                await transaction.RollbackAsync();

                // Optional: log error or return to error page
                ModelState.AddModelError("", "An error occurred while saving the settlements.");
                return RedirectToAction(nameof(TechnicianSummary));
            }

            // Redirect to GET to prevent form resubmission
            return RedirectToAction(nameof(TechniciansSettlement));

        }


        [HttpGet]

        public async Task<IActionResult> TechniciansSettlement()
        {
            var techSettlments = await _context.TechnicianSettlements.ToListAsync();
            return View(techSettlments);
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
                    .Where(i => i.CompanyId == company.Id && !i.IsComplete && !i.IsSealed && !i.IsCanceled)
                    .ToListAsync();
                if (!insurances.Any()) continue;

                var totalInsuranceAmount = insurances.Sum(i => i.InsuranceAmount);

                viewModels.Add(new InsuranceCompanySettlement
                {
                    ProviderId = company.Id,
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
        public async Task<IActionResult> MarkAsComplete(InsuranceCompanySettlement insuraceCSettelment)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Insurances
               .Where(r => r.CompanyId == insuraceCSettelment.ProviderId && !r.IsSealed && !r.IsCanceled)
               .ExecuteUpdateAsync(setters => setters
               .SetProperty(r => r.IsComplete, true));


                await _context.InsuranceCompanySettlements.AddAsync(insuraceCSettelment);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Optional: redirect instead of returning View (to prevent re-submission)
                return RedirectToAction(nameof(InsuranceCompanySettlement));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "An error occurred while completing the settlement.");
                return RedirectToAction(nameof(InsuranceCompanySummary));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsuranceCompanySettlement(List<InsuranceCompanySettlement> Settlements)
        {
            if (Settlements.Sum(i => i.TotalInsuranceShare) == 0)
                return View(await _context.PartnerSettlements.ToListAsync());


            await _context.Insurances
            .Where(r => r.IsComplete && !r.IsTechnicianShared && !r.IsSealed && !r.IsCanceled)
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
            try
            {
                var targetDate = DateTime.Today;

                var partners = await _context.Partners
                    .AsNoTracking()
                    .ToListAsync();

                var summaries = new List<PartnerSettlement>();

                // Pre-calculate total expenses
                var totalExpenses = await _context.Expenditures
                    .AsNoTracking()
                    .Where(e => !e.IsSealed)
                    .SumAsync(e => e.Amount);
                // Load all valid expenditures with service link
                var allExpenses = await _context.Expenditures
                    .AsNoTracking()
                    .Where(e => !e.IsSealed && e.RadiologyTypeId != null)
                    .ToListAsync();

                // Cache Reservations, Insurances, and Debts in memory (untracked)
                var allReservations = await _context.Reservations
                    .AsNoTracking()
                    .Where(r => !r.IsSealed && !r.IsCanceled)
                    .ToListAsync();

                var allInsurances = await _context.Insurances
                    .AsNoTracking()
                    .Where(i => !i.IsSealed && !i.IsCanceled && i.IsComplete)
                    .ToListAsync();

                var allDebts = await _context.Debts
                    .AsNoTracking()
                    .Where(d => !d.IsSealed && !d.IsCanceled && d.IsPaid)
                    .ToListAsync();

                foreach (var partner in partners)
                {
                    decimal partnerTotalShare = 0;
                    decimal partnerExpenseShare = 0;

                    var partnerServices = await _context.PartnerServices
                        .AsNoTracking()
                        .Where(ps => ps.PartnerId == partner.Id)
                        .ToListAsync();

                    foreach (var ps in partnerServices)
                    {
                        int serviceId = ps.ServiceId;
                        decimal percentage = ps.Amount_Percentage / 100m;

                        // === EXPENSES ===
                        var serviceExpenses = allExpenses.Where(e => e.RadiologyTypeId == serviceId);
                        decimal serviceTotalExpense = serviceExpenses.Sum(e => e.Amount);
                        partnerExpenseShare += serviceTotalExpense * percentage;
                        // === RESERVATIONS ===
                        var serviceReservations = allReservations
                            .Where(r => r.RadiologyTypeId == serviceId);

                        decimal reservationAmount = serviceReservations.Sum(r => r.PaiedAmount);
                        decimal reservationTechShare = serviceReservations.Sum(r => r.TechnicianShare);
                        decimal reservationNet = reservationAmount - reservationTechShare;

                        // === INSURANCES ===
                        var serviceInsurances = (from i in allInsurances
                                                 join r in allReservations on i.ReservationId equals r.Id
                                                 where r.RadiologyTypeId == serviceId
                                                 select i).ToList();

                        decimal insuranceAmount = serviceInsurances.Sum(i => i.InsuranceAmount);
                        decimal insuranceTechShare = serviceInsurances.Sum(i => i.TechnicianShare);
                        decimal insuranceNet = insuranceAmount - insuranceTechShare;

                        // === DEBTS ===
                        var serviceDebts = (from d in allDebts
                                            join r in allReservations on d.ReservationId equals r.Id
                                            where r.RadiologyTypeId == serviceId
                                            select d).ToList();

                        decimal debtAmount = serviceDebts.Sum(d => d.Amount);
                        decimal debtTechShare = serviceDebts.Sum(d => d.TechnicianShare);
                        decimal debtNet = debtAmount - debtTechShare;

                        // === TOTAL SERVICE NET INCOME ===
                        decimal serviceNet = reservationNet + insuranceNet + debtNet;

                        // === PARTNER SHARE ===
                        partnerTotalShare += serviceNet * percentage;
                    }

                    // Deduct average expense share
                    decimal expenseShare = totalExpenses / partners.Count;
                    decimal netPartnerAmount = partnerTotalShare - partnerExpenseShare;

                    summaries.Add(new PartnerSettlement
                    {
                        PartnerName = partner.PartnerName,
                        Amount = Math.Round(netPartnerAmount, 2),
                        SettlementDate = targetDate
                    });
                }

                ViewBag.SummaryDate = targetDate.ToShortDateString();
                return View("PartnerSummary", summaries);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IActionResult> OPartnerSummary()
        {

            var targetDate = DateTime.Today;
            var partners = await _context.Partners.ToListAsync();
            var summaries = new List<PartnerSettlement>();

            var reservationsPaiedamount = await _context.Reservations
               .Where(r => !r.IsSealed && !r.IsCanceled)
               .SumAsync(r => r.PaiedAmount);

            var reservationsTchentionShareAmount = await _context.Reservations
               .Where(r => !r.IsSealed && !r.IsCanceled)
               .SumAsync(r => r.TechnicianShare);

            var insurancesPaiedamount = await _context.Insurances
                .Where(r => r.IsComplete && r.IsTechnicianShared && !r.IsSealed && !r.IsCanceled)
                .SumAsync(r => r.InsuranceAmount);

            var insurancesTchentionShareAmount = await _context.Insurances
               .Where(r => r.IsTechnicianShared && !r.IsSealed && !r.IsCanceled)
               .SumAsync(r => r.TechnicianShare);

            var debtsTchentionShareAmount = await _context.Debts
               .Where(r => r.IsTechnicianShared && !r.IsSealed && !r.IsCanceled)
               .SumAsync(r => r.TechnicianShare);

            var debtspaiedAmount = await _context.Debts
               .Where(r => r.IsPaid && r.IsTechnicianShared && !r.IsSealed && !r.IsCanceled)
               .SumAsync(r => r.Amount);


            var expenses = await _context.Expenditures
                .Where(r => !r.IsSealed)
                .SumAsync(r => r.Amount);
            //var partner = await _context.Partners;


            foreach (var partner in partners)
            {
                var partnerServices = await _context.PartnerServices
                    .Where(ps => ps.PartnerId == partner.Id)
                    .ToListAsync();
                var partnerPercentage = partnerServices.Sum(s => s.Amount_Percentage);

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
            .Where(r => !r.IsSealed && !r.IsCanceled)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsSealed, true));

            await _context.Insurances
            .Where(r => r.IsComplete && r.IsTechnicianShared && !r.IsSealed && !r.IsCanceled)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(r => r.IsSealed, true));

            await _context.Debts
            .Where(r => r.IsTechnicianShared && r.IsPaid && !r.IsSealed && !r.IsCanceled)
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