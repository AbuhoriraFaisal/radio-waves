using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using ImagingLabApp.Models;
using radio_waves.Models;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
namespace radio_waves.Data
{
   

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<RadiologyType> RadiologyTypes { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Expenditure> Expenditures { get; set; }
        public DbSet<Debt> Debts { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<PartnerSettlement> PartnerSettlements { get; set; }
        public DbSet<InsuranceCompanies> InsuranceCompanies { get; set; }
    }

}
