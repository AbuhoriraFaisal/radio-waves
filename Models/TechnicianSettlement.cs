namespace radio_waves.Models
{
    public class TechnicianSettlement
    {
        public int Id { get; set; }
        public int TechnicianId { get; set; }
        public string TechnicianName { get; set; }

        public decimal TotalFromReservations { get; set; }
        public decimal TotalFromInsurance { get; set; }
        public decimal TotalDebtShare { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetPayable { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        public Technician Technician { get; set; }
        public DateTime SettelmentDate { get; set; } = DateTime.UtcNow;
    }

}
