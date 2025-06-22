using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace radio_waves.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        [ValidateNever]
        public Patient Patient { get; set; }
        public DateTime AppointmentDate { get; set; }

        public int RadiologyTypeId { get; set; }
        [ValidateNever]
        public RadiologyType RadiologyType { get; set; }
        [ValidateNever]
        public Technician Technician { get; set; }
        public int TechnicianId { get; set; }
        [ValidateNever]
        public Shift Shift { get; set; }
        public int ShiftId { get; set; }
        public decimal BasePrice { get; set; }
        public DateTime ReservationDate =DateTime.Now;
        
        public decimal PaiedAmount { get; set; }
        public bool IsPaid => Payments?.Sum(p => p.Amount) >= PaiedAmount;
        public ICollection<Payment> Payments { get; set; }

        public bool IsSealed { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsDebt { get; set; }
        public bool IsCommission { get; set; }
        public bool CoveredByInsurance { get; set; }
        public int? InsuranceId { get; set; }
        public decimal TechnicianShare { get; set; } // = TotalPrice * TechnicianPercentage at booking time
        //public string 
        public Reservation()
        {
            Payments = new HashSet<Payment>();
        }
    }

}
