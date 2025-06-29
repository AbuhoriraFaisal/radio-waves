using System.ComponentModel.DataAnnotations;

namespace radio_waves.Models
{
    public class Technician
    {
        public int Id { get; set; }
        [Display(Name = "Technician")]
        public string FullName { get; set; }
        public string Certification { get; set; }
        public bool IsAvailable { get; set; }
    }

}
