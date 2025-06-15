using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace radio_waves.Models
{
    public class Patient : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Phone { get; set; }
        public bool HasInsurance { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public int? InsuranceId { get; set; }
        [ValidateNever]  // 
        public InsuranceCompanies Insurance { get; set; }
        public Patient()
        {
            Reservations = new HashSet<Reservation>();
            Payments = new HashSet<Payment>();
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HasInsurance && InsuranceId == null)
            {
                yield return new ValidationResult("Insurance must be selected when HasInsurance is checked.", new[] { "InsuranceId" });
            }
        }
    }
}
