namespace radio_waves.Models
{
    public class TechnicianSettlementViewModel
    {
        public int Id { get; set; }
        public string TechnicianName { get; set; }
        public decimal TotalFromReservations { get; set; }
        public decimal TotalFromInsurance { get; set; }
        public List<InsuranceBreakdown> InsuranceDetails { get; set; }
        public decimal TotalDebtShare { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetPayable { get; set; }
        public DateTime SettelmentDate { get; set; }

    }

    public class InsuranceBreakdown
    {
        public int Id { get; set; }
        public string InsuranceCompany { get; set; }
        public decimal TechnicianShare { get; set; }
    }

}
