using System.ComponentModel;

namespace radio_waves.Models
{
    public class Insurance
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public InsuranceCompanies Company { get; set; }
        public int? PatientId { get; set; }
        public Patient Patient { get; set; }
        [DisplayName("Reservation No")]
        public int? ReservationId { get; set; }
        public int? TechnicianId { get; set; }
        public decimal TechnicianShare { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public string PolicyNumber { get; set; }
        public bool IsComplete { get; set; }
        public bool IsTechnicianShared { get; set; }
        public bool IsSealed { get; set; }
        public bool IsCanceled { get; set; } = false; // Default to false, indicating not canceled
        //public DateTime InsuranceDate = DateTime.Now;

    }
}
