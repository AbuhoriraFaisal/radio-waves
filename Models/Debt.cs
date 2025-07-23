using System.ComponentModel.DataAnnotations;

namespace radio_waves.Models
{
    public class Debt
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        [Display(Name = "Patient")]
        public Patient Patient { get; set; }
        [Display(Name = "Reservation No")]
        public int? ReservationId { get; set; }
        public int? TechnicianId { get; set; }
        public Technician Technician { get; set; }
        public decimal TechnicianShare { get; set; }
        public Reservation Reservation { get; set; }
        public string Comments { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }

        [Display(Name = "Paid")]
        public bool IsPaid { get; set; }
        //public DateTime DebtDate = DateTime.Now;
        public bool IsTechnicianShared { get; set; }
        public bool IsSealed { get; set; }
        public bool IsCanceled { get; set; } = false; // Default to false, indicating not canceled
    }

}
