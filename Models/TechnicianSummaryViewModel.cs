public class TechnicianSummaryViewModel
{
    public string TechnicianName { get; set; }
    public decimal TotalFromReservations { get; set; }
    public List<InsuranceBreakdownItem> InsuranceBreakdown { get; set; }
    public decimal TotalFromInsurance { get; set; }
    public decimal TotalDebtShare { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetPayable { get; set; }
}

public class InsuranceBreakdownItem
{
    public string InsuranceCompany { get; set; }
    public decimal TechnicianShare { get; set; }
}
