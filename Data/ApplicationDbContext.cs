using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using radio_waves.Models;
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
        public DbSet<TechnicianSettlement> TechnicianSettlements { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerServices> PartnerServices { get; set; }

        public DbSet<InsuranceCompanySettlement> InsuranceCompanySettlements { get; set; }

        //public DbSetTechnicianSettlement> TechnicianSettlements { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PartnerServices>()
    .HasIndex(ps => new { ps.PartnerId, ps.ServiceId })
    .IsUnique();
        }


    }

}
