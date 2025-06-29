using System.ComponentModel.DataAnnotations;

namespace radio_waves.Models
{
    public class RadiologyType
    {
        public int Id { get; set; }
        [Display(Name = "Service Type")]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

}
